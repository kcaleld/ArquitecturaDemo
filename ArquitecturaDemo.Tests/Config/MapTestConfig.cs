namespace SistemaILP.Test.LibroIVA.Config
{
    public class MapTestConfig
    {
        private readonly IMapper _mapper;

        public MapTestConfig()
        {
            if (_mapper == null)
            {
                var mapConfig = new MapperConfiguration(config =>
                {
                    config.AddProfile(new ConfigureMaps());
                });

                var mapper = mapConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        public IMapper Get() => _mapper;
    }
}