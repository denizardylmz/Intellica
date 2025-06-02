using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace NoteAPI.Services.Models.OllamaModels
{
    public class OllamaRequest
    {
        public OllamaRequest(string model, string prompt)
        {
            Model = model;
            Prompt = prompt;
            Options = new();
        }
        public OllamaRequest(string model, string prompt, string system)
        {
            Model = model;
            Prompt = prompt;
            System = system;
            Options = new();
        }

        public OllamaRequest(string model, string prompt, string system, OllamaOptions options)
        {
            Model = model;
            Prompt = prompt;
            System = system;
            Options = options;
        }

        public string Model { get; set; }    // Model adı
        public string Prompt { get; set; }   // Kullanıcının girdisi
        public string System { get; set; }   // (Opsiyonel) Sistem promptu
        public List<int> Context { get; set; }   // (Opsiyonel) Token bazlı bağlam
        public OllamaOptions Options { get; set; }   // (Opsiyonel) Üretim ayarları
    }

    public class OllamaOptions
    {
        public OllamaOptions()
        {
            
        }
        public OllamaOptions(double temperature, double topP, int maxTokens, List<string> Stop = null)
        { 
            Temperature= temperature;
            TopP= topP;
            MaxTokens= maxTokens;
            this.Stop = Stop;
        }

        public double Temperature { get; set; } = 0.7;    // Rastgelelik seviyesi
        public double TopP { get; set; } = 0.9;           // Çekirdek örnekleme
        public int MaxTokens { get; set; } = 200;         // Maksimum üretilecek token
        public List<string> Stop { get; set; }            // Yanıtı durduracak dizeler
    }
}
