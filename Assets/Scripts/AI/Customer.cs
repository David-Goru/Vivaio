using System.Collections.Generic;
using UnityEngine;

public class Customer
{
    public string Name;
    public int Age;
    public string Job;
    public string[] Priorities;
    public int MediumAmount;

    public int Trust;

    GameObject body;
    Stand nextStand;
    Stack<Vector2> path;
    Stack<CustomerDesire> customerDesires;
    CustomerDesire currentDesire;
    int expenses;
    float desireTimer;
    bool paid;
    float speed;
    string lastDir;

    public Customer(GameObject body, float speed)
    {
        this.body = body;
        body.SetActive(false);
        customerDesires = new Stack<CustomerDesire>();
        
        this.speed = speed;
        Trust = 95;
        lastDir = "Idle down";
    }

    void CheckDesire()
    {
        if (desireTimer == 1)
        {            
            Vector2 distance = new Vector2(nextStand.Model.transform.position.x - body.transform.position.x,
                                            nextStand.Model.transform.position.y - body.transform.position.y);
            if (distance.x > 0 && lastDir != "Idle right")
            { 
                body.GetComponent<Animator>().SetTrigger("IdleRight");
                lastDir = "Idle right";
            }
            else if (distance.x < 0 && lastDir != "Idle left")
            {
                body.GetComponent<Animator>().SetTrigger("IdleLeft");
                lastDir = "Idle left";
            }
            else if (distance.y > 0 && lastDir != "Idle up")
            {
                body.GetComponent<Animator>().SetTrigger("IdleUp");
                lastDir = "Idle up";
            }
            else if (distance.y < 0 && lastDir != "Idle down")
            {
                body.GetComponent<Animator>().SetTrigger("IdleDown");
                lastDir = "Idle down";
            }
        }

        desireTimer -= Time.deltaTime;
        
        if (!nextStand.Available || nextStand.Item == null)
        {
            customerDesires.Push(currentDesire);
            desireTimer = 1f;
            NextDesire();
        }
        else if (desireTimer <= 0)
        {
            if (currentDesire.MaxPrice >= nextStand.ItemValue) expenses += nextStand.Take(currentDesire.Amount);

            desireTimer = 1f;
            NextDesire();
        }
    }

    void NextDesire()
    {
        nextStand = null;

        while (customerDesires.Count > 0 && nextStand == null)
        {
            nextStand = Stands.StandsList.Find(x => x.Item == customerDesires.Peek().Item);
            if (nextStand != null && nextStand.Available == false) nextStand = null;
            customerDesires.Pop();
        }

        if (nextStand == null) path = VertexSystem.Route(body.transform.position, AI.CashRegister);
        else path = VertexSystem.Route(body.transform.position, nextStand.CustomerPos);
    }

    public void UpdateState()
    {
        if (path.Count == 0)
        {
            if (!paid && nextStand == null)
            {
                if (expenses > 0)
                {
                    MonoBehaviour.Instantiate(Resources.Load<GameObject>("Shop/Coin animation"));
                    if (Trust < 85) Trust++;
                }
                else if (Trust > 1) Trust--;
                Master.UpdateBalance(expenses);
                paid = true;
                path = VertexSystem.Route(body.transform.position, AI.End);
            }                
            else if (paid) RemoveCustomer();
            else if (desireTimer > 0) CheckDesire();
        }
        else
        {
            if ((Vector2)body.transform.position == path.Peek())
            { 
                path.Pop();
                if (path.Count == 0) return;

                Vector2 distance = new Vector2(path.Peek().x - body.transform.position.x,
                                               path.Peek().y - body.transform.position.y);
                if (distance.x > 0 && lastDir != "Walking right")
                { 
                    body.GetComponent<Animator>().SetTrigger("WalkRight");
                    lastDir = "Walking right";
                }
                else if (distance.x < 0 && lastDir != "Walking left") 
                {
                    body.GetComponent<Animator>().SetTrigger("WalkLeft");
                    lastDir = "Walking left";
                }
                else if (distance.y > 0 && lastDir != "Walking up")
                { 
                    body.GetComponent<Animator>().SetTrigger("WalkUp");
                    lastDir = "Walking up";
                }
                else if (distance.y < 0 && lastDir != "Walking down")
                {
                    body.GetComponent<Animator>().SetTrigger("WalkDown");
                    lastDir = "Walking down";
                }
            }
            else body.transform.position = Vector2.MoveTowards(body.transform.position, path.Peek(), Time.deltaTime * speed);
        }
    }

    void RemoveCustomer()
    {
        AI.UnavailableCustomers.Add(this);
        AI.ActiveCustomers.Remove(this);
        body.SetActive(false);
    }

    public void ActivateCustomer()
    {        
        body.transform.position = new Vector2(-15, -5);
        int totalDesires = Random.Range(1, Priorities.Length + 1);
        for (int i = 0; i < totalDesires; i++)
        {
            customerDesires.Push(new CustomerDesire(Products.ProductsList.Find(x => x.Name == Priorities[i]), MediumAmount));
        }

        while (customerDesires.Count > 0 && nextStand == null)
        {
            currentDesire = customerDesires.Peek();
            nextStand = Stands.StandsList.Find(x => x.Item == currentDesire.Item);
            customerDesires.Pop();
        }

        if (nextStand == null) path = VertexSystem.Route(body.transform.position, AI.End);
        else path = VertexSystem.Route(body.transform.position, nextStand.CustomerPos);

        expenses = 0;
        desireTimer = 1;
        paid = false;

        body.SetActive(true);
        AI.ActiveCustomers.Add(this);
        AI.AvailableCustomers.Remove(this);
    }
}