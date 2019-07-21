using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public enum Food { Hamburger, Number9, Cheese, Dip };

public class Game_Manager : MonoBehaviour
{
    public  Text            ui_customers_text;
    public  Text            ui_inqueue_text;
    public  InputField      inputField;
    private int             inqueue_number;
    private float           next_action_time = 0.0f;
    private float           period = 3.0f;
    private List<string>    customer_list = new List<string>();
    private List<Customer>  customers= new List<Customer>();
    public  int             id = 1;
    // Start is called before the first frame update
    void Start()
    {
        inqueue_number = 0;
        ui_inqueue_text.text = "0";
        ui_customers_text.text = "";
        var se= new InputField.SubmitEvent();
        se.AddListener(fuck);
        inputField.onEndEdit = se;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > next_action_time){
            next_action_time += period;
            Customer new_customer = new Customer(id++);
            inqueue_number += 1;
            ui_inqueue_text.text = inqueue_number.ToString();
            customers.Add(new_customer);
            ui_customers_text.text = generate_customer_list_text(update_customers_list(new_customer, true));
        }

        if (inqueue_number > 3){
            SceneManager.LoadScene("game_over_scene");
        }
    }

    private void fuck(string arg0){
        if( Int32.Parse(arg0) == customers[0].get_price()){
            ui_customers_text.text = generate_customer_list_text(update_customers_list(null, false));        
        }
        else {
            Debug.Log("you're wrong fucker");
        }
    }

    private List<string> update_customers_list(Customer customer, bool isnew){
        List<string> temp = customer_list;
        if(isnew){
            temp.Add(customer.get_id().ToString() + ":>" + customer.get_order().ToString());
        }
        else {
            customers.RemoveAt(0);
            temp.RemoveAt(0);
            inqueue_number--;
            ui_inqueue_text.text = inqueue_number.ToString();
        }
        return temp;
    }

    private string generate_customer_list_text(List<string> customer_list){
        return string.Join( "\n",customer_list);
    }
}




public class Customer{
    public int customer_id;

    private List<Food> Order = new List<Food>();
    public Customer(int id){
        customer_id = id;
        Order = generate_order();
    }
    
    public int get_id() { return this.customer_id; }
    public string get_order() { 
        List<string> stringList = this.Order.ConvertAll(Food => Food.ToString());
        string res = string.Join(",", stringList);
        return res; 
    }
    private List<Food> generate_order(){
        System.Random r = new System.Random();
        int     numbers = r.Next(1 , 4);
        List<Food> result = new List<Food>();
        Array values = Enum.GetValues(typeof(Food));
        for(int i = 0; i < numbers ; i++){
            result.Add((Food)values.GetValue(r.Next(values.Length)));
        }
        return result;
    }

    public int get_price(){
        int sum = 0;
        foreach(Food food in Order){
            if ( food == Food.Dip ){
                sum += 1;
            }
            else if ( food == Food.Cheese ){
                sum += 2;
            }
            else if ( food == Food.Hamburger ){
                sum += 3;                
            }
            else if ( food == Food.Number9 ){
                sum += 4;
            }
        }
        
        return sum;
    }
    
}


