namespace AreaProject16_6_2025.ViewModel
{
    public class RunwayViewModel
    {
        public int Id { get; set; }
        public string ThresholdCoordinates { get; set; } // إحداثيات نقطة بداية المدرج (العرض، الطول، الارتفاع)
        public int airpotId { get; set; }
        //public string RWName { get; set; } // اسم المدرج (مثلاً RW17L تعني المدرج 17 يسار)
        //public double RWLength { get; set; } // طول المدرج بالأمتار
        //public double RWWidth { get; set; } // عرض المدرج بالأمتار
        //public string ThresholdCoordinates { get; set; } // إحداثيات نقطة بداية المدرج (العرض، الطول، الارتفاع)

        #region Runway Region

        public double RunwayWidth { get; set; }
        public double RunwayLength { get; set; }

        public double RunwayStartLatitude { get; set; }
        public double RunwayStartLongitude { get; set; }

        public double RunwayEndLatitude { get; set; }
        public double RunwayEndLongitude { get; set; }

        public string RunwayName { get; set; }
        #endregion

    }
}

