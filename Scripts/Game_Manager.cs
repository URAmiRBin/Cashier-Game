using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public enum Food { Hamburger, Number9, Cheese, Dip };

public class Game_Manager : MonoBehaviour
{
    // Time Variables
    private float           next_action_time = 0.0f;
    private float           period = 3.0f;
    // UI Variables
    public  Text            ui_customers_text;
    public  Text            ui_inqueue_text;
    public  InputField      inputField;
    public  Text            ui_score;
    // Scripting Variables
    private int             inqueue_number;
    private int             score;
    private List<string>    customer_list = new List<string>();
    private List<Customer>  customers= new List<Customer>();
    public  int             id = 1;
    // Start is called before the first frame update
    void Start()
    {
        inqueue_number = 0;
        score = 0;
        ui_inqueue_text.text = "Customers wating : 0";
        ui_customers_text.text = "";
        ui_score.text = "0 $";
        // This is for InputField Listener
        var se= new InputField.SubmitEvent();
        se.AddListener(SubmitField);
        inputField.onEndEdit = se;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Run this every 3 seconds
        if (Time.time > next_action_time){
            next_action_time += period;
            Customer new_customer = new Customer(id++);
            inqueue_number += 1;
            ui_inqueue_text.text = "Customers wating : " + inqueue_number.ToString();
            customers.Add(new_customer);
            ui_customers_text.text = generate_customer_list_text(update_customers_list(new_customer, true));
        }

        // Checks Game Over State
        if (inqueue_number > 3){
            SceneManager.LoadScene("game_over_scene");
        }
    }

    // Runs when Inputfield Event Listener is triggred
    private void SubmitField(string arg0){
        // Checks if given number is correct
        if ( Int32.Parse(arg0) == customers[0].get_price()){
            ui_customers_text.text = generate_customer_list_text(update_customers_list(null, false));
            score += 1;
        }
        else {
            Debug.Log("you're wrong fucker");
            score -= 1;
        }
        ui_score.text = score.ToString() + " $";
    }

    // Updates Customers List: THIS IS UI TEST
    private List<string> update_customers_list(Customer customer, bool isnew){
        List<string> temp = customer_list;
        if(isnew){
            temp.Add(customer.CustomerId.ToString() + ":>" + customer.get_order().ToString());
        }
        else {
            customers.RemoveAt(0);
            temp.RemoveAt(0);
            inqueue_number--;
            ui_inqueue_text.text = inqueue_number.ToString();
        }
        return temp;
    }

    // Generates a string from customers list : THIS IS UI TEST
    private string generate_customer_list_text(List<string> customer_list){
        return string.Join( "\n",customer_list);
    }
}




public class Customer{
    // Fields
    private int _customerId;
    private List<Food> _order;
    
    // Properties
    public int CustomerId 
    {
        get
        {
            return _customerId;
        }
    }

    // Constructor
    public Customer(int id){
        _customerId = id;
        _order = generate_order();
    }
    
    // Fucntions
    public string get_order() { 
        List<string> stringList = _order.ConvertAll(Food => Food.ToString());
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
        foreach(Food food in _order){
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


