using System;
using System.Diagnostics;

namespace LrndefLib
{
    /// <summary>
    /// Represents a simplified and smaller version number in the format Major.Minor.Build.Revision. Based on <see cref="Version"/>.
    /// </summary>
    public readonly struct SimpleVersion :
        IEquatable<SimpleVersion>,
        IEquatable<Version>,
        IEquatable<FileVersionInfo>,
        IComparable,
        IComparable<SimpleVersion>,
        IComparable<Version>,
        IComparable<FileVersionInfo>,
        ICloneable
    {
        /// <summary>
        /// Represents this object's value packed into an <see cref="int"/>.
        /// </summary>
        public int PackedValue { get; }

        /// <summary>
        /// Represents this version's major component.
        /// </summary>
        public byte Major => (byte)(PackedValue >> 24);

        /// <summary>
        /// Represents this version's minor component.
        /// </summary>
        public byte Minor => (byte)((PackedValue >> 16) & 0xFF);

        /// <summary>
        /// Represents this version's build component.
        /// </summary>
        public byte Build => (byte)((PackedValue >> 8) & 0xFF);

        /// <summary>
        /// Represents this version's revision component.
        /// </summary>
        public byte Revision => (byte)(PackedValue & 0xFF);

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleVersion"/> class with the specified major, minor, build, and revision numbers.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <param name="build">The build number.</param>
        /// <param name="revision">The revision number.</param>
        public SimpleVersion(
            byte major,
            byte minor,
            byte build = 0,
            byte revision = 0)
        {
            PackedValue = (major << 24) | (minor << 16) | (build << 8) | (revision);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleVersion"/> class with the specified packed value.
        /// </summary>
        /// <param name="packedValue">The packed value.</param>
        public SimpleVersion(
            int packedValue)
        {
            PackedValue = packedValue;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Major}.{Minor}.{Build}.{Revision}";
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return PackedValue;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is null.</exception>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            switch (obj)
            {
                case SimpleVersion simpleVersion:
                    return Equals(simpleVersion);
                case Version version:
                    return Equals(version);
                case FileVersionInfo fileVersionInfo:
                    return Equals(fileVersionInfo);
                default:
                    return false;
            }
        }

        /// <inheritdoc />
        public bool Equals(SimpleVersion other)
        {
            return PackedValue.Equals(other.PackedValue);
        }

        /// <inheritdoc />
        public bool Equals(Version other)
        {
            if (other is null)
            {
                return false;
            }

            return (Major == other.Major || Major == 0 && other.Major == -1)
                && (Minor == other.Minor || Minor == 0 && other.Minor == -1)
                && (Build == other.Build || Build == 0 && other.Build == -1)
                && (Revision == other.Revision || Revision == 0 && other.Revision == -1);
        }

        /// <inheritdoc />
        public bool Equals(FileVersionInfo other)
        {
            if (other is null)
            {
                return false;
            }

            return Major == other.FileMajorPart
                && Minor == other.FileMinorPart
                && Build == other.FileBuildPart
                && Revision == other.FilePrivatePart;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="obj"/> is null.</exception>
        /// <exception cref="ArgumentException"><see cref="SimpleVersion"/> is not comparable with <paramref name="obj"/>.</exception>
        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            switch (obj)
            {
                case SimpleVersion simpleVersion:
                    return CompareTo(simpleVersion);
                case Version version:
                    return CompareTo(version);
                case FileVersionInfo fileVersionInfo:
                    return CompareTo(fileVersionInfo);
                default:
                    throw new ArgumentException($"{nameof(SimpleVersion)} Not comparable with argument.", nameof(obj));
            }
        }

        /// <inheritdoc />
        public int CompareTo(SimpleVersion other)
        {
            return PackedValue.CompareTo(other.PackedValue);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public int CompareTo(Version other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (Major != other.Major || Major == 0 && other.Major == -1)
            {
                if (Major > other.Major)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            if (Minor != other.Minor || Minor == 0 && other.Minor == -1)
            {
                if (Minor > other.Minor)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            if (Build != other.Build || Build == 0 && other.Build == -1)
            {
                if (Build > other.Build)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            if (Revision != other.Revision || Revision == 0 && other.Revision == -1)
            {
                if (Revision > other.Revision)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            return 0;
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is null.</exception>
        public int CompareTo(FileVersionInfo other)
        {
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            if (Major != other.FileMajorPart)
            {
                if (Major > other.FileMajorPart)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            if (Minor != other.FileMinorPart)
            {
                if (Minor > other.FileMinorPart)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            if (Build != other.FileBuildPart)
            {
                if (Build > other.FileBuildPart)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            if (Revision != other.FilePrivatePart)
            {
                if (Revision > other.FilePrivatePart)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            return 0;
        }

        /// <inheritdoc />
        public object Clone()
        {
            return new SimpleVersion(PackedValue);
        }

        /// <summary>
        /// Determines whether two specified <see cref="SimpleVersion"/> objects are equal.
        /// </summary>
        /// <param name="a">The first <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The second <see cref="SimpleVersion"/> object.</param>
        /// <returns>true if <paramref name="a"/> equals <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator ==(SimpleVersion a, SimpleVersion b)
        {
            return a.PackedValue == b.PackedValue;
        }

        /// <summary>
        /// Determines whether two specified <see cref="SimpleVersion"/> objects are not equal.
        /// </summary>
        /// <param name="a">The first <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The second <see cref="SimpleVersion"/> object.</param>
        /// <returns>true if <paramref name="a"/> does not equal <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator !=(SimpleVersion a, SimpleVersion b)
        {
            return a.PackedValue != b.PackedValue;
        }

        /// <summary>
        /// Determines whether the first specified <see cref="SimpleVersion"/> object is less than the second specified <see cref="SimpleVersion"/> object.
        /// </summary>
        /// <param name="a">The first <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The second <see cref="SimpleVersion"/> object.</param>
        /// <returns>true if <paramref name="a"/> is less than <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator <(SimpleVersion a, SimpleVersion b)
        {
            return a.PackedValue < b.PackedValue;
        }

        /// <summary>
        /// Determines whether the first specified <see cref="SimpleVersion"/> object is less than or equal to the second specified <see cref="SimpleVersion"/> object.
        /// </summary>
        /// <param name="a">The first <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The second <see cref="SimpleVersion"/> object.</param>
        /// <returns>true if <paramref name="a"/> is less than or equal to <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator <=(SimpleVersion a, SimpleVersion b)
        {
            return a.PackedValue <= b.PackedValue;
        }

        /// <summary>
        /// Determines whether the first specified <see cref="SimpleVersion"/> object is greater than the second specified <see cref="SimpleVersion"/> object.
        /// </summary>
        /// <param name="a">The first <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The second <see cref="SimpleVersion"/> object.</param>
        /// <returns>true if <paramref name="a"/> is greater than <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator >(SimpleVersion a, SimpleVersion b)
        {
            return a.PackedValue > b.PackedValue;
        }

        /// <summary>
        /// Determines whether the first specified <see cref="SimpleVersion"/> object is greater than or equal to the second specified <see cref="SimpleVersion"/> object.
        /// </summary>
        /// <param name="a">The first <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The second <see cref="SimpleVersion"/> object.</param>
        /// <returns>true if <paramref name="a"/> is greater than or equal to <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator >=(SimpleVersion a, SimpleVersion b)
        {
            return a.PackedValue >= b.PackedValue;
        }

        /// <summary>
        /// Determines whether the specified <see cref="SimpleVersion"/> object is equal to the specified <see cref="Version"/> object.
        /// </summary>
        /// <param name="a">The <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The <see cref="Version"/> object.</param>
        /// <returns>true if <paramref name="a"/> equals <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator ==(SimpleVersion a, Version b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Determines whether the specified <see cref="SimpleVersion"/> object is not equal to the specified <see cref="Version"/> object.
        /// </summary>
        /// <param name="a">The <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The <see cref="Version"/> object.</param>
        /// <returns>true if <paramref name="a"/> does not equal <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator !=(SimpleVersion a, Version b)
        {
            return !a.Equals(b);
        }

        /// <summary>
        /// Determines whether the specified <see cref="SimpleVersion"/> object is less than the specified <see cref="Version"/> object.
        /// </summary>
        /// <param name="a">The <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The <see cref="Version"/> object.</param>
        /// <returns>true if <paramref name="a"/> is less than <paramref name="b"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="b"/> is null.</exception>
        public static bool operator <(SimpleVersion a, Version b)
        {
            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            var place = a.CompareTo(b);
            if (place >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="SimpleVersion"/> object is less than or equal to the specified <see cref="Version"/> object.
        /// </summary>
        /// <param name="a">The <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The <see cref="Version"/> object.</param>
        /// <returns>true if <paramref name="a"/> is less than or equal to <paramref name="b"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="b"/> is null.</exception>
        public static bool operator <=(SimpleVersion a, Version b)
        {
            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            var place = a.CompareTo(b);
            if (place > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="SimpleVersion"/> object is greater than the specified <see cref="Version"/> object.
        /// </summary>
        /// <param name="a">The <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The <see cref="Version"/> object.</param>
        /// <returns>true if <paramref name="a"/> is greater than <paramref name="b"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="b"/> is null.</exception>
        public static bool operator >(SimpleVersion a, Version b)
        {
            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            var place = a.CompareTo(b);
            if (place <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="SimpleVersion"/> object is greater than or equal to the specified <see cref="Version"/> object.
        /// </summary>
        /// <param name="a">The <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The <see cref="Version"/> object.</param>
        /// <returns>true if <paramref name="a"/> is greater than or equal to <paramref name="b"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="b"/> is null.</exception>
        public static bool operator >=(SimpleVersion a, Version b)
        {
            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            var place = a.CompareTo(b);
            if (place < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="SimpleVersion"/> object is equal to the specified <see cref="FileVersionInfo"/> object.
        /// </summary>
        /// <param name="a">The <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The <see cref="FileVersionInfo"/> object.</param>
        /// <returns>true if <paramref name="a"/> equals <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator ==(SimpleVersion a, FileVersionInfo b)
        {
            return a.Major == b.FileMajorPart
                && a.Minor == b.FileMinorPart
                && a.Build == b.FileBuildPart
                && a.Revision == b.FilePrivatePart;
        }

        /// <summary>
        /// Determines whether the specified <see cref="SimpleVersion"/> object is not equal to the specified <see cref="FileVersionInfo"/> object.
        /// </summary>
        /// <param name="a">The <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The <see cref="FileVersionInfo"/> object.</param>
        /// <returns>true if <paramref name="a"/> does not equal <paramref name="b"/>; otherwise, false.</returns>
        public static bool operator !=(SimpleVersion a, FileVersionInfo b)
        {
            return a.Major != b.FileMajorPart
                || a.Minor != b.FileMinorPart
                || a.Build != b.FileBuildPart
                || a.Revision != b.FilePrivatePart;
        }

        /// <summary>
        /// Determines whether the specified <see cref="SimpleVersion"/> object is less than the specified <see cref="FileVersionInfo"/> object.
        /// </summary>
        /// <param name="a">The <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The <see cref="FileVersionInfo"/> object.</param>
        /// <returns>true if <paramref name="a"/> is less than <paramref name="b"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="b"/> is null.</exception>
        public static bool operator <(SimpleVersion a, FileVersionInfo b)
        {
            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            var place = a.CompareTo(b);
            if (place >= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="SimpleVersion"/> object is less than or equal to the specified <see cref="FileVersionInfo"/> object.
        /// </summary>
        /// <param name="a">The <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The <see cref="FileVersionInfo"/> object.</param>
        /// <returns>true if <paramref name="a"/> is less than or equal to <paramref name="b"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="b"/> is null.</exception>
        public static bool operator <=(SimpleVersion a, FileVersionInfo b)
        {
            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            var place = a.CompareTo(b);
            if (place > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="SimpleVersion"/> object is greater than the specified <see cref="FileVersionInfo"/> object.
        /// </summary>
        /// <param name="a">The <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The <see cref="FileVersionInfo"/> object.</param>
        /// <returns>true if <paramref name="a"/> is greater than <paramref name="b"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="b"/> is null.</exception>
        public static bool operator >(SimpleVersion a, FileVersionInfo b)
        {
            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            var place = a.CompareTo(b);
            if (place <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="SimpleVersion"/> object is greater than or equal to the specified <see cref="FileVersionInfo"/> object.
        /// </summary>
        /// <param name="a">The <see cref="SimpleVersion"/> object.</param>
        /// <param name="b">The <see cref="FileVersionInfo"/> object.</param>
        /// <returns>true if <paramref name="a"/> is greater than or equal to <paramref name="b"/>; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="b"/> is null.</exception>
        public static bool operator >=(SimpleVersion a, FileVersionInfo b)
        {
            if (b is null)
            {
                throw new ArgumentNullException(nameof(b));
            }

            var place = a.CompareTo(b);
            if (place < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Converts a <see cref="Version"/> object into a <see cref="SimpleVersion"/> object.
        /// </summary>
        /// <param name="version">A <see cref="Version"/> object.</param>
        /// <returns>A <see cref="SimpleVersion"/> that represents the converted <see cref="Version"/> object.</returns>
        /// <remarks><see cref="Version"/> uses integers for version fields, while <see cref="SimpleVersion"/> uses bytes. <c>2017.10.2</c> would become <c>255.10.2</c>.</remarks>
        public static implicit operator SimpleVersion(Version version)
        {
            return new SimpleVersion(
                (byte)(version.Major == -1 ? 0 : version.Major),
                (byte)(version.Minor == -1 ? 0 : version.Minor),
                (byte)(version.Build == -1 ? 0 : version.Build),
                (byte)(version.Revision == -1 ? 0 : version.Revision));
        }

        /// <summary>
        /// Converts a <see cref="FileVersionInfo"/> object into a <see cref="SimpleVersion"/> object.
        /// </summary>
        /// <param name="version">A <see cref="FileVersionInfo"/> object.</param>
        /// <returns>A <see cref="SimpleVersion"/> that represents the converted <see cref="FileVersionInfo"/> object.</returns>
        /// <remarks><see cref="FileVersionInfo"/> uses integers for version fields, while <see cref="SimpleVersion"/> uses bytes. <c>2017.10.2</c> would become <c>255.10.2</c>.</remarks>
        public static implicit operator SimpleVersion(FileVersionInfo version)
        {
            return new SimpleVersion(
                (byte)version.FileMajorPart,
                (byte)version.FileMinorPart,
                (byte)version.FileBuildPart,
                (byte)version.FilePrivatePart);
        }

        /// <summary>
        /// Converts a <see cref="SimpleVersion"/> object into a <see cref="Version"/> object.
        /// </summary>
        /// <param name="version">A <see cref="SimpleVersion"/> object.</param>
        /// <returns>A <see cref="Version"/> that represents the converted <see cref="SimpleVersion"/> object.</returns>
        public static implicit operator Version(SimpleVersion version)
        {
            return new Version(
                version.Major,
                version.Minor,
                version.Build,
                version.Revision);
        }

        /// <summary>
        /// Converts a <see cref="SimpleVersion"/> object into an <see cref="int"/> object.
        /// </summary>
        /// <param name="version">A <see cref="SimpleVersion"/> object.</param>
        /// <returns>An <see cref="int"/> that represents the converted <see cref="SimpleVersion"/> object.</returns>
        public static explicit operator int(SimpleVersion version)
        {
            return version.PackedValue;
        }
    }
}
