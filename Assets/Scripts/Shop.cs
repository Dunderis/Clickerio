using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Baker")]
    public TextMeshProUGUI priceText;
    public int price = 10;
    public TextMeshProUGUI countText;
    public int count = 0;

    public int cpb = 1;
    public float bakerSpeed = 2f;

    private float s;
    

    private Clicker clicker;
    

    private void Start() 
    {
        price = PlayerPrefs.GetInt("price", 10);
        count = PlayerPrefs.GetInt("cook", 0);
        countText.text = count.ToString();
        priceText.text = $"Price: {price}";

        clicker = FindObjectOfType<Clicker>();
        InvokeRepeating("Cook", 0, bakerSpeed);
        


    }


    public void BuyBaker()
    {
        if (clicker.clicks >= price)
        {
            clicker.clicks -= price;
            UiManager.instance.UpdateClicks(clicker.clicks);
            
            
            count++;
            countText.text = count.ToString();

            price = (int)(price * 1.1f);//price increase 10%;
            priceText.text = $"Price: {price}";
        }
    }


    void Cook(){
        clicker.clickVFX.Emit(cpb*count);
        clicker.clicks += cpb*count;
        UiManager.instance.UpdateClicks(clicker.clicks);
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            Save();
        }
    }
    private void OnApplicationQuit()
    {
        Save();
    }
    public void Save()
    {
        PlayerPrefs.SetInt("cook", count);
        PlayerPrefs.SetInt("price", price);
        PlayerPrefs.Save();
    }

}
