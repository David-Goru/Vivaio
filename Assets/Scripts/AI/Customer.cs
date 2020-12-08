﻿using System.Collections.Generic;
using UnityEngine;
using CodeTools;

public class Customer
{
    int id;
    public string Name;
    public int Age;
    public string Job;
    public string[] Priorities;
    public int MediumAmount;

    public int Trust;
    public bool LetterSent;

    GameObject body;
    Stand nextStand;
    Vector2 pathStart;
    Vector2 pathEnd;
    Stack<Vector2> path;
    Stack<CustomerDesire> customerDesires;
    CustomerDesire currentDesire;
    int expenses;
    string itemsBought;
    int numberItemsBought;
    float desireTimer;
    bool paid;
    float speed;
    string lastDir;

    public Customer(int id, GameObject body, float speed)
    {
        this.id = id;
        this.body = body;
        body.SetActive(false);
        customerDesires = new Stack<CustomerDesire>();
        
        this.speed = speed;
        Trust = 95;
        LetterSent = false;
        lastDir = "Idle down";
        itemsBought = "";
        numberItemsBought = 0;
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
            if (currentDesire.MaxPrice >= nextStand.ItemValue)
            {
                Tuple info = nextStand.Take(currentDesire.Amount);
                string amountTaken = info.Item1;
                int itemCost = info.Item2;

                if (amountTaken != "0")
                {
                    expenses += itemCost;
                    itemsBought += string.Format("{0} x{1} ({2}€)\n", Localization.Translations[currentDesire.Item.Name], amountTaken, itemCost);                    
                    numberItemsBought++;
                }
            }

            desireTimer = 1f;
            NextDesire();
        }
    }

    void NextDesire()
    {
        nextStand = null;

        while (customerDesires.Count > 0 && nextStand == null)
        {
            nextStand = Master.Data.Stands.Find(x => x.Available == true && x.Item == customerDesires.Peek().Item);
            currentDesire = customerDesires.Peek();
            customerDesires.Pop();
        }

        if (nextStand == null)
        {
            path = VertexSystem.FindPath(body.transform.position, CashRegisterHandler.CustomerPos);
            if (path.Count == 0) // Can't reach the cash register
            {
                if (Trust > 10) Trust--;
                CashRegisterHandler.Data.CashLog.Add(new ShopTicket(Name, expenses, itemsBought, numberItemsBought));
                MonoBehaviour.Instantiate(Resources.Load<GameObject>("Shop/Coin animation"), CashRegisterHandler.CashRegisterModel.transform);
                Master.UpdateBalance(expenses);
            }
        }
        else path = VertexSystem.FindPath(body.transform.position, nextStand.CustomerPos);
    }

    public void UpdateState()
    {
        if (path.Count == 0)
        {
            if (!paid && nextStand == null)
            {
                if (expenses > 0)
                {
                    CashRegisterHandler.Data.CashLog.Add(new ShopTicket(Name, expenses, itemsBought, numberItemsBought));
                    MonoBehaviour.Instantiate(Resources.Load<GameObject>("Shop/Coin animation"), CashRegisterHandler.CashRegisterModel.transform);
                    if (Trust < 85) Trust++;
                    Master.UpdateBalance(expenses);
                    Master.RunSoundStatic(CashRegisterHandler.CashRegisterModel.GetComponent<CashRegisterHandler>().Clip);
                }
                else if (Trust > 10) Trust--;
                paid = true;
                path = VertexSystem.FindPath(body.transform.position, pathEnd);
                if (path.Count == 0) // Can't leave the shop
                {
                    if (Trust > 10) Trust--;
                    RemoveCustomer();
                }
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
            else body.transform.position = Vector2.MoveTowards(body.transform.position, path.Peek(), Time.deltaTime * speed * TimeSystem.Data.TimeSpeed);
        }
    }

    void RemoveCustomer()
    {
        AI.ActiveCustomers.Remove(id);
        body.SetActive(false);
        lastDir = "Idle down";
    }

    public void ActivateCustomer()
    {
        pathStart = AI.CustomerPositions[Random.Range(0, AI.CustomerPositions.Count)];
        pathEnd = AI.CustomerPositions[Random.Range(0, AI.CustomerPositions.Count)];
        while (pathEnd.Equals(pathStart))
        {
            pathEnd = AI.CustomerPositions[Random.Range(0, AI.CustomerPositions.Count)];
        }

        body.transform.position = pathStart;

        /*
        if (Shop.Data.Inaugurated)
        {
            [...]
        }
        else
        {
            path = VertexSystem.FindPath(body.transform.position, pathEnd);
            paid = true;
        }
        */

        // That goes inside the [...] thing
        int totalDesires = Random.Range(1, Priorities.Length + 1);
        for (int i = 0; i < totalDesires; i++)
        {
            customerDesires.Push(new CustomerDesire(Products.ProductsList.Find(x => x.Name == Priorities[i]), MediumAmount));
        }

        while (customerDesires.Count > 0 && nextStand == null)
        {
            currentDesire = customerDesires.Peek();
            nextStand = Master.Data.Stands.Find(x => x.Available == true && x.Item == currentDesire.Item);
            customerDesires.Pop();

            if (nextStand != null)
            {
                path = VertexSystem.FindPath(body.transform.position, nextStand.CustomerPos);
                if (path.Count == 0) nextStand = null;
            }
        }

        if (nextStand == null) path = VertexSystem.FindPath(body.transform.position, pathEnd);

        expenses = 0;
        itemsBought = "";
        numberItemsBought = 0;
        desireTimer = 1;
        paid = false;
        // Until here

        body.SetActive(true);
        AI.ActiveCustomers.Add(id);
    }
}