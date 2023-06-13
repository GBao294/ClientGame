using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;


public class APIScript : MonoBehaviour
{
    [SerializeField] private InputField IpField;
   // xoa player khoi database 
   public async void SendDeleteRequest(string id)
    {
        using (HttpClient client = new HttpClient())
        {
          
        HttpResponseMessage response = await client.DeleteAsync($"http://" + IpField.text.ToString() + ":3000/Player/"+id);
        response.EnsureSuccessStatusCode();
        // Kiểm tra phản hồi từ API và trả về kết quả tùy thuộc vào yêu cầu của bạn
        // return response.IsSuccessStatusCode;
         
        }
    }

    async Task<HttpResponseMessage> SendPostRequest(string apiUrl, object data)
    {
        using (HttpClient client = new HttpClient())
        {
          
                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                return response;
           
        }
    }

    // Đoạn code sử dụng phương thức SendPostRequest để gửi dữ liệu mới lên API
    public async Task<bool> SendNewData(string user, string pass)
    {
        var newPost = new
        { 
            name = user,
            password = pass
        };
        HttpResponseMessage response = await SendPostRequest("http://"+  IpField.text.ToString() + ":3000/Player", newPost);
        // Kiểm tra thành công của yêu cầu
        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //Hàm để so sánh thông tin với dữ liệu trên API
    public async Task<bool> GET(string name, string pass)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync("http://"+  IpField.text.ToString() + ":3000/Player");
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            List<User> users = JsonConvert.DeserializeObject<List<User>>(responseContent);

            foreach (User user in users)
            {
                if (user.name == name && user.password == pass)
                {
                   
                    return true;
                }
                
            }
        }
        return false;
    }

    async Task<HttpResponseMessage> SendPutRequest(string apiUrl, object data)
    {
        using (HttpClient client = new HttpClient())
        {
                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();     
                return response;
         
        }
    }
    
    // Đoạn code sử dụng phương thức SendPutRequest để gửi dữ liệu cập nhật lên API
    async Task SendUpdateData()
    {
            var updatedPost = new
            {
                id = 10,
                title = "XXXX",
                author = "TTTT"
            };

            HttpResponseMessage response = await SendPutRequest("http://localhost:3000/Player/1", updatedPost);
    }
    public class User
    {

        [JsonProperty("id")]
        public int id { get; set; }


        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }



    }
}

