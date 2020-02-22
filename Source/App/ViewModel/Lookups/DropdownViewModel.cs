namespace Project.ViewModel
{
    public class DropdownViewModel
    {
        public DropdownViewModel()
        {
            
        }

        public DropdownViewModel(string id, string name, string code = "")
        {
            Id = id;
            Name = name;
            Code = code;
        }        
        public string Id { get; set; }        
        public string Name { get; set; }
        public string Code { get; set; }

        public int Identity { get; set; }
        public string ExtraData { get; set; }
        public string MetaData { get; set; }
    }
}