using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Diagnostics;

namespace Utils
{
    [Serializable, XmlRoot("Version")]
    public class VersionClone
    {
        private Version version;
        private int build;
        private int major;
        private short majorRevision;
        private int minor;
        private short minorRevision;
        private int revision;

        public VersionClone(Version version)
        {
            this.version = version;
        }

        public VersionClone(XElement element)
        {
            foreach (var subElement in element.Elements())
            {
                switch (subElement.Name.LocalName)
                {
                    case "Build":
                        build = int.Parse(subElement.Value);
                        break;
                    case "Major":
                        major = int.Parse(subElement.Value);
                        break;
                    case "MajorRevision":
                        majorRevision = short.Parse(subElement.Value);
                        break;
                    case "Minor":
                        minor = int.Parse(subElement.Value);
                        break;
                    case "MinorRevision":
                        minorRevision = short.Parse(subElement.Value);
                        break;
                    case "Revision":
                        revision = int.Parse(subElement.Value);
                        break;
                    default:
                        Debugger.Break();
                        break;
                }
            }
        }

        public VersionClone()
        {
        }

        public int Build
        {
            get
            {
                if (version != null)
                {
                    return version.Build;
                }
                else
                {
                    return build;
                }
            }

            set
            {
                build = value;
            }
        }

        public int Major
        {
            get
            {
                if (version != null)
                {
                    return version.Major;
                }
                else
                {
                    return major;
                }
            }

            set
            {
                major = value;
            }
        }

        public short MajorRevision
        {
            get
            {
                if (version != null)
                {
                    return version.MajorRevision;
                }
                else
                {
                    return majorRevision;
                }
            }

            set
            {
                majorRevision = value;
            }
        }

        public int Minor
        {
            get
            {
                if (version != null)
                {
                    return version.Minor;
                }
                else
                {
                    return minor;
                }
            }

            set
            {
                minor = value;
            }
        }

        public short MinorRevision
        {
            get
            {
                if (version != null)
                {
                    return version.MinorRevision;
                }
                else
                {
                    return minorRevision;
                }
            }

            set
            {
                minorRevision = value;
            }
        }

        public int Revision
        {
            get
            {
                if (version != null)
                {
                    return version.Revision;
                }
                else
                {
                    return revision;
                }
            }

            set
            {
                revision = value;
            }
        }
    }
}
