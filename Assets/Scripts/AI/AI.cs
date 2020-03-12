using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class AI : MonoBehaviour
{
    public static List<Customer> AvailableCustomers;
    public static List<Customer> UnavailableCustomers;
    public static List<Customer> ActiveCustomers;
    public static Vector3 End;
    public static Vector3 CashRegister;

    float nextCustomer;

    void Start()
    {
        End = new Vector3(9.5f, 12.5f, 0);
        CashRegister = new Vector3(-13.5f, 10.5f, 0);

        nextCustomer = Random.Range(3, 20);

        AvailableCustomers = new List<Customer>();
        UnavailableCustomers = new List<Customer>();
        ActiveCustomers = new List<Customer>();

        XmlDocument customersDoc = new XmlDocument();
        customersDoc.Load(Application.dataPath + "/Data/CustomersInfo.xml");
        XmlNodeList customers = customersDoc.GetElementsByTagName("Customer");
        
        foreach (XmlNode customer in customers)
        {       
            Customer c = new Customer((GameObject)Instantiate(Resources.Load("Customers/" + customer["Name"].InnerText), new Vector2(-15, -5), Quaternion.Euler(0, 0, 0)), float.Parse(customer["Speed"].InnerText, NumberStyles.Float, new CultureInfo("en-US")));

            c.Name = customer["Name"].InnerText;
            c.Age = int.Parse(customer["Age"].InnerText);
            c.Job = customer["Job"].InnerText;

            List<string> priorities = new List<string>();

            foreach (XmlNode priority in customer["Priorities"].ChildNodes)
            {
                priorities.Add(priority.InnerText);
            }

            c.Priorities = new string[priorities.Count];
            for (int i = 0; i < priorities.Count; i++)
            {
                c.Priorities[i] = priorities[i];
            }

            c.MediumAmount = int.Parse(customer["MediumAmount"].InnerText);

            AvailableCustomers.Add(c);
        }
    }

    void Update()
    {
        nextCustomer -= Time.deltaTime;
        if (AvailableCustomers.Count > 0 && nextCustomer <= 0)
        {
            if (TimeSystem.TimeState == "Shop open")
            {
                nextCustomer = Random.Range(3, 20);
                Customer customer = AvailableCustomers[Random.Range(0, AvailableCustomers.Count - 1)];
                
                Debug.Log(customer.Trust);
                if (Random.Range(0, 100) < customer.Trust) customer.ActivateCustomer();
                else
                {
                    UnavailableCustomers.Add(customer);
                    AvailableCustomers.Remove(customer);
                }
            }
        }

        foreach (Customer customer in ActiveCustomers.ToArray())
        {
            customer.UpdateState();
        }
    }

    public void NewDay()
    {
        nextCustomer = Random.Range(3, 20);
        foreach (Customer c in UnavailableCustomers)
        {
            AvailableCustomers.Add(c);
        }
        UnavailableCustomers.Clear();
    }
}