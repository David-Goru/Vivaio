using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class AI : MonoBehaviour
{
    public static List<Customer> Customers;
    public static List<int> AvailableCustomers;
    public static List<int> ActiveCustomers;
    public static float NextCustomer;

    public static List<Vector2> CustomerPositions;

    // When loading a game
    public static bool Load(AIData data)
    {
        try
        {
            CustomerPositions = new List<Vector2>();
            foreach (Transform t in GameObject.Find("Initializer").transform.Find("Customer positions"))
            {
                CustomerPositions.Add(t.position);
            }

            GetCustomers();

            foreach (Customer c in Customers)
            {
                CustomerData d = data.Customers.Find(x => x.Name == c.Name);
                if (d != null)
                {
                    c.Trust = d.Trust;
                    c.LetterSent = d.LetterSent;
                }
            }

            GameObject.Find("Farm handler").GetComponent<AI>().enabled = true;
        }
        catch (System.Exception e)
        {
            GameLoader.Log.Add(string.Format("Failed loading {0}. Error: {1}", "AI", e));
        }

        return true;
    }

    // When creating a new game
    public static bool New()
    {
        CustomerPositions = new List<Vector2>();
        foreach (Transform t in GameObject.Find("Initializer").transform.Find("Customer positions"))
        {
            CustomerPositions.Add(t.position);
        }
        
        GetCustomers();
        GameObject.Find("Farm handler").GetComponent<AI>().enabled = true;

        return true;
    }

    // When saving the game
    public static AIData Save()
    {
        AIData data = new AIData();
        data.Customers = new List<CustomerData>();

        foreach (Customer c in Customers)
        {
            data.Customers.Add(new CustomerData(c.Name, c.Trust, c.LetterSent));
        }

        return data;
    }

    public static void GetCustomers()
    {
        NextCustomer = Random.Range(3, 20);

        Customers = new List<Customer>();
        AvailableCustomers = new List<int>();
        ActiveCustomers = new List<int>();

        XmlDocument customersDoc = new XmlDocument();
        customersDoc.Load(Application.dataPath + "/Data/CustomersInfo.xml");
        XmlNodeList customers = customersDoc.GetElementsByTagName("Customer");

        int customerCount = 0;        
        foreach (XmlNode customer in customers)
        {       
            Customer c = new Customer(customerCount, (GameObject)Instantiate(Resources.Load("Customers/" + customer["Name"].InnerText), new Vector2(-15, -5), Quaternion.Euler(0, 0, 0)), float.Parse(customer["Speed"].InnerText, NumberStyles.Float, new CultureInfo("en-US")));

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

            Customers.Add(c);
            AvailableCustomers.Add(customerCount);
            customerCount++;
        }
    }

    void Update()
    {
        NextCustomer -= Time.deltaTime * TimeSystem.Data.TimeSpeed;
        if (AvailableCustomers.Count > 0 && NextCustomer <= 0 && (TimeSystem.Data.TimeState == TimeState.SHOP_OPEN || TimeSystem.Data.TimeState == TimeState.EVENING))
        {
            int customerID = AvailableCustomers[Random.Range(0, AvailableCustomers.Count - 1)];
            Customer customer = Customers[customerID];
            
            AvailableCustomers.Remove(customerID);
            NextCustomer = Random.Range(3, 20);

            if (Random.Range(0, 100) < customer.Trust) customer.ActivateCustomer();
        }

        foreach (int i in ActiveCustomers.ToArray())
        {
            Customers[i].UpdateState();
        }
    }

    public static void NewDay()
    {
        NextCustomer = Random.Range(3, 20);
        AvailableCustomers = new List<int>();
        for (int i = 0; i < Customers.Count; i++)
        {
            if (!ActiveCustomers.Contains(i)) AvailableCustomers.Add(i);
        }
    }
}