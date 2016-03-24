using System.Xml;

namespace EnvironmentMonitor
{
    public interface IXmlProvider
    {
        void Export(XmlWriter writer);
    }
}