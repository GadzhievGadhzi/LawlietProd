namespace Lawliet.Models {
    public class PricePosition {
        public string Problem { get; set; }
        public string Price { get; set; }
    }

    public class PhoneModel {
        public PhoneModel() {
            PricePositions = new List<PricePosition>();
        }

        public string Title { get; set; }
        public List<PricePosition> PricePositions { get; set; }
    }

    public class PhoneBrand {
        public PhoneBrand() {
            PhoneModels = new List<PhoneModel>();
        }

        public string Title { get; set; }
        public List<PhoneModel> PhoneModels { get; set; }
    }

    public class PriceViewModel {
        public PriceViewModel() {
            PhoneBrands = new List<PhoneBrand>();
        }

        public List<PhoneBrand> PhoneBrands { get; set; }

        //кол-во ошибок при импорте
        public int ErrorsTotal { get; set; }
    }
}