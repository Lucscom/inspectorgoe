using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using GameComponents;

namespace InspectorGoe.ViewModels;
public partial class MainViewModel : ObservableObject
{
    private Controller controller = Controller.GetInstance();
    private List<Player> detectives;

    public MainViewModel()
    {
        controller.Initialize(4);

        detectives = new List<Player>(controller.Detectives);

        var X = detectives.First();
        X.ScooterTicket = 3;
        X.BikeTicket = 3;
        X.BusTicket = 3;
    }

    public List<Player> DetectivesCollection
    {
        get { return detectives; }
        set { detectives = value; }
    }




}
