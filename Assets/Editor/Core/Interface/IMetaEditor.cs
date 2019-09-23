namespace XCFramework.Editor
{
    public interface IMetaEditor
    {
        void OnGUI();

        bool Initialize();

        void OnFocus();
    }
}