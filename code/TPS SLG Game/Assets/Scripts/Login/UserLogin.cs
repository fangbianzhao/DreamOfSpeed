﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UserLogin : MonoBehaviour {

    private NetworkHost networkHost;
    private bool loginSuccessfully = false;

    public InputField nameInputField;
    public InputField passwordInputField;
    public Text tips;


	// Use this for initialization
	void Start () {
        networkHost = NetworkHost.GetInstance();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        tips.text = "";
    }
	
	// Update is called once per frame
	void Update () {
        if (loginSuccessfully)
            return;
        networkHost.ReceiveData();
        while (networkHost.receiveMessages.Count > 0)
        {
            ServerMsg serverMessage = networkHost.receiveMessages.Dequeue();
            Debug.Log("receive server msg type: " + Convert.ToString(serverMessage.msgType, 16));

            if (serverMessage.msgType == NetworkSettings.SERVER_FEEDBACK)
            {

                switch (((ServerFeedbackMsg)serverMessage).code)
                {
                    case NetworkSettings.COMMAND_LOGIN_SUCCESSFUL:
                        loginSuccessfully = true;
                        Debug.Log("jump to gamescene");
                        SceneManager.LoadScene("Halloween_Level");
                        break;
                    case NetworkSettings.COMMAND_SEND_CLIENTID:
                        Debug.Log(Convert.ToString(((ServerClientIDMsg)serverMessage).clientID));
                        GameSettings.clientID = ((ServerClientIDMsg)serverMessage).clientID;
                        break;
                    case NetworkSettings.COMMAND_REGISTER_SUCCESSFUL:
                        tips.text = "[Accept] Register Successfully!";
                        break;
                    case NetworkSettings.COMMAND_WRONG_PASSWORD:
                        tips.text = "[Wrong] Wrong Password!";
                        break;
                    case NetworkSettings.COMMAND_NAME_ALREADY_EXISTS:
                        tips.text = "[Wrong] This Name Already Exist!";
                        break;
                    case NetworkSettings.COMMAND_DATABASE_ERROR:
                        tips.text = "[Wrong] Database Wrong in Server";
                        break;
                    case NetworkSettings.COMMAND_LOGIN_ALREADY:
                        tips.text = "[Wrong] User Login Already!";
                        break;
                }
            }    

        }
    }
    //登录
    public void Login()
    {
        // add check the user input
        string userName = nameInputField.text;
        string password = passwordInputField.text;

        byte[] msg = MessageHandler.SetLoginMsg(userName, password);
        //Debug.Log("send msg: " + BitConverter.ToString(msg));
        StartCoroutine (networkHost.SendBytesMessage(msg));
        GameSettings.username = userName;
    }
    //注册
    public void Register()
    {
        string userName = nameInputField.text;
        string password = passwordInputField.text;
        if(userName.Length > 0 && password.Length > 0 && userName.Length < 10 && password.Length < 10)
        {
            byte[] msg = MessageHandler.SetRegisterMsg(userName, password);
            //Debug.Log("send msg: " + BitConverter.ToString(msg));
            StartCoroutine(networkHost.SendBytesMessage(msg));
        }
        else{
            print("请输入合法用户名和密码");
        }
    }
    //测试账号
    public void TestLogin()
    {
        GameSettings.username = "test";
        byte[] msg = MessageHandler.SetLoginMsg("test", "163");
        StartCoroutine(networkHost.SendBytesMessage(msg));
    }
}
