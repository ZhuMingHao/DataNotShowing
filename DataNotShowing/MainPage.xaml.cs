using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DataNotShowing
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

     
        public MainPage()
        {
        
            this.InitializeComponent();
                   
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            AudioFXModel = await AudioFXViewModel.CreateAsync();
            base.OnNavigatedTo(e);
        }
    }

    public class CheckBoxData
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
    }

    public class AudioFXViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if(PropertyChanged != null)
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(propertyName)));
            }
          
        }

        private bool _fNeedsProcessing = false;
        public bool NeedsProcessing { get => _fNeedsProcessing; }

        private ObservableCollection<CheckBoxData> _FXList;
        public ObservableCollection<CheckBoxData> FXList
        {
            get
            {
                return _FXList;
            }

            set
            {
                _FXList = value;
                this.OnPropertyChanged();
            }
        }

        public AudioFXViewModel() {
            FXList = new ObservableCollection<CheckBoxData>();
            audioFXViewModel = this;
        }
        private static AudioFXViewModel audioFXViewModel;

        // Originally public static AudioFXViewModel Create()
        public static async Task<AudioFXViewModel> CreateAsync()
        {
                   
           
            await audioFXViewModel.LoadFXContentAsync();
            return audioFXViewModel;
        }

        // Originally private void LoadFXContent()
        private async Task LoadFXContentAsync()
        {
            // Originally using File.OpenText()
            StorageFile jsonFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Data/Settings.json"));

            using (StreamReader sr = new StreamReader(await jsonFile.OpenStreamForReadAsync()))
            {
                var json = sr.ReadToEnd();
                var data = JsonConvert.DeserializeObject<FXData>(json);
                foreach (string s in data.FXList)
                {
                    FXList.Add(new CheckBoxData() { Name = s, Selected = true });
                }
                           
                _fNeedsProcessing = true;
            }
        }
    }
}
