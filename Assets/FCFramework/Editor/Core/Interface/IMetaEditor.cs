namespace XCFramework.Editor
{
    public interface IMetaEditor
    {
        /// <summary>
        /// 展示gui 需要主动调用
        /// </summary>
        void OnGUI();

        /// <summary>
        /// 展示前的数据初始化
        /// </summary>
        /// <returns></returns>
        bool Initialize();

        void OnFocus();
    }
}