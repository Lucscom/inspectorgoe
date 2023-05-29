using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectorGoe.ViewModels
{
    public class StudentViewModel
    {

        private List<Students> studentsCollection = new List<Students>();

        public StudentViewModel()
        {
            studentsCollection.Add(new Students("Marie", "White", "+1-809-554-6055"));
            studentsCollection.Add(new Students("Paola", "Pullman", "+1-809-506-5655"));
            studentsCollection.Add(new Students("Joseph", "McDonalds", "+1-809-684-4876"));
            studentsCollection.Add(new Students("Max", "Gangbanger", "+125234-5655"));
            studentsCollection.Add(new Students("Moritz", "Bumsluemmel", "+23455532-4876"));
        }

        public List<Students> StudentsCollection
        {
            get { return studentsCollection; }
            set { studentsCollection = value; }
        }
    }
}
