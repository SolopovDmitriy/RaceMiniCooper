using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsMenuScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputFieldMass;
    [SerializeField] private float defaultMass = 2000;

    [SerializeField] private TMP_InputField _inputFieldForce;
    [SerializeField] private float defaultForce = 2000;

    // Start is called before the first frame update
    void Start()
    {
        float mass = PlayerPrefs.GetFloat("mass", defaultMass);
        _inputFieldMass.text = mass.ToString();

        float force = PlayerPrefs.GetFloat("force", defaultForce);
        _inputFieldForce.text = force.ToString();
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    public void SaveParameters()
    {
        try
        {
            float mass = float.Parse(_inputFieldMass.text);
            PlayerPrefs.SetFloat("mass", mass);

            float force = float.Parse(_inputFieldMass.text);
            PlayerPrefs.SetFloat("force", force);
        }
        catch (System.Exception)
        {
            Debug.Log("_inputField is not a number");            
        }
    }





}
