using Zenject;
public class ApplicationLauncher
{
    private readonly UIService _uiService;

    public ApplicationLauncher(
        UIService uiService,
        IInstantiator instantiator)
    {
        instantiator.InstantiatePrefabResource("Player/Player");
        instantiator.InstantiatePrefabResource("Level/Level");

        _uiService = uiService;
        _uiService.Init();

        _uiService.Show<UIMenu>();                        
    }
}
