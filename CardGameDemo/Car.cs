using System;
using System.Collections.Generic;
using System.Text;

namespace CardGameDemo
{
    public abstract class Vehicle
    {
        protected bool _isDriving;

        public abstract int MaxPassengers {
            get;
        }
    }

    public class Bus : Vehicle
    {
        public override int MaxPassengers {
            get { return 40; }
        }
    }

    public class Car : Vehicle
    {
        //Members
        private int _fuelRemaining;

        //Properties
        public override int MaxPassengers { get { return 5; } }
        public string Make { get; private set; }
        public string Model { get; private set; }

        //Events
        public event Action<Car> HasStarted;
        public event Action<Car> HasStopped;

        //Methods
        public void Drive()
        {
            _isDriving = true;
            _fuelRemaining = _fuelRemaining - 1; // _fuelRemaining--;
            HasStarted?.Invoke(this);
        }

        public void Stop()
        {
            _isDriving = false;
            HasStopped?.Invoke(this);
        }

        //Constructor
        public Car(string Make, string Model)
        {
            this.Make = Make; this.Model = Model;
            _isDriving = false;_fuelRemaining = 100;
        }
    }
}
