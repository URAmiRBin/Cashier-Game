using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public enum Food { Apple, Cheese, Cookie, Bacon };
public class Game_Manager : MonoBehaviour
{
    // Time Variables
    private float           next_action_time = 0.0f;
    private float           period = 3.0f;
    // UI Variables
    public  Text            ui_customers_text;
    public  Text            ui_inqueue_text;
    public  Text            ui_score;
    public  Text            ui_input_text;
    public  GameObject      sprite_whole;
    // Scripting Variables
    private int             inqueue_number;
    private int             score;
    private List<string>    customer_list = new List<string>();
    private List<Customer>  customers= new List<Customer>();
    // Start is called before the first frame update
    void Start()
    {
        inqueue_number = 0;
        score = 0;
        ui_input_text.text = "";
        ui_inqueue_text.text = "Customers wating : 0";
        ui_customers_text.text = "";
        ui_score.text = "0 $";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Run this every 3 seconds
        if (Time.time > next_action_time){
            next_action_time += period;
            System.Random r = new System.Random();
            period = (float)(r.NextDouble() * (6 - 3) + 3);
            Customer new_customer = new Customer(inqueue_number,sprite_whole);
            inqueue_number += 1;
            ui_inqueue_text.text = "Customers wating : " + inqueue_number.ToString();
            customers.Add(new_customer);
            ui_customers_text.text = generate_customer_list_text(update_customers_list(new_customer, true));
            new_customer.ui_order();
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
            score += customers[0].get_price();
            customers[0].ui_remove();
            ui_update();
            ui_customers_text.text = generate_customer_list_text(update_customers_list(null, false));
            
        }
        else {
            Debug.Log("you're wrong fucker");
            score -= 5;
        }
        ui_score.text = score.ToString() + " $";
    }

    private void ui_update(){
        foreach(Customer customer in customers){
            if(customer.CustomerId != 0){
                customer.setId(customer.CustomerId - 1);
                customer.ui_remove();
                customer.update_cv(sprite_whole);
                customer.ui_order();
            }
            
        }
        
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


    public void keyboard1(){
        ui_input_text.text = ui_input_text.text + "1";
    }
    public void keyboard2(){
        ui_input_text.text = ui_input_text.text + "2";
    }
    public void keyboard3(){
        ui_input_text.text = ui_input_text.text + "3";
    }
    public void keyboard4(){
        ui_input_text.text = ui_input_text.text + "4";
    }
    public void keyboard5(){
        ui_input_text.text = ui_input_text.text + "5";
    }
    public void keyboard6(){
        ui_input_text.text = ui_input_text.text + "6";
    }
    public void keyboard7(){
        ui_input_text.text = ui_input_text.text + "7";
    }
    public void keyboard8(){
        ui_input_text.text = ui_input_text.text + "8";
    }
    public void keyboard9(){
        ui_input_text.text = ui_input_text.text + "9";
    }
    public void keyboard0(){
        ui_input_text.text = ui_input_text.text + "0";
    }
    public void keyboarde(){
        SubmitField(ui_input_text.text);
        ui_input_text.text = "";
    }
}




public class Customer{
    // Fields
    private int _customerId;
    private List<Food> _order;
    private GameObject _customerSprite;
    private List<GameObject> _orderSprite;
    // Properties
    public int CustomerId 
    {
        get
        {
            return _customerId;
        }
    }

    public void setId(int newid){
        _customerId = newid;
    }

    // Constructor
    public Customer(int id, GameObject spritewhole){
        _customerId = id;
        _order = generate_order();
        _customerSprite = spritewhole.transform.GetChild(id).gameObject;
        _orderSprite = new List<GameObject>();
        for( int i = 0; i < _order.Count; i++){
            _orderSprite.Add(_customerSprite.transform.GetChild(i).gameObject);            
        } 
    }
    
    // Fucntions
    public string get_order() { 
        List<string> stringList = _order.ConvertAll(Food => Food.ToString());
        string res = string.Join(",", stringList);
        return res; 
    }

    public void update_cv(GameObject spritewhole){
        _customerSprite = spritewhole.transform.GetChild(_customerId).gameObject;
        _orderSprite = new List<GameObject>();
        for( int i = 0; i < _order.Count; i++){
            _orderSprite.Add(_customerSprite.transform.GetChild(i).gameObject);            
        }
    }

    public void ui_order() {
        for( int i = 0; i < _order.Count; i++){
            _orderSprite[i].GetComponent<sprite_spawner>().LoadSprite(_order[i], (float)_customerId, (float)i);
        } 
    }

    public void ui_remove(){
        for( int i = 0; i < _order.Count; i++){
            _orderSprite[i].GetComponent<sprite_spawner>().DeloadSprite();
        } 
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
            if ( food == Food.Apple ){
                sum += 1;
            }
            else if ( food == Food.Cheese ){
                sum += 2;
            }
            else if ( food == Food.Cookie ){
                sum += 3;                
            }
            else if ( food == Food.Bacon ){
                sum += 4;
            }
        }
        
        return sum;
    }
    
}


