namespace Tu_Deuda.ViewModel
{
    public class Language
    {
        private readonly string _language = "EN";
        private readonly string _nameText = "Name";
        private readonly string _valueText = "Value";
        private readonly string _descriptionText = "Description";
        private readonly string _add_clientText = "Add Client";
        private readonly string _seach_ClientText = "Search Client";

        private readonly string _idioma = "ES";
        private readonly string _nombreText = "Nombre";
        private readonly string _valorText = "Valor";
        private readonly string _descripcionText = "Descripcion";
        private readonly string _añadir_clienteText = "Añadir Cliente";
        private readonly string _buscar_ClienteText = "Buscar Cliente";

        public string LanguageApp { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string AddClient { get; set; }
        public string Date { get; set; }
        public string SearchClient { get; set; }
        public string Credit { get; set; }
        public string Hour { get; set; }
    }
}