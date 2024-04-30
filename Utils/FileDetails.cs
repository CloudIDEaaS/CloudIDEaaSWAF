using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Shell32;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.ShellProperties;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;
using System.Threading;

namespace Utils
{
    public class FilePropertyDataTypeAttribute : Attribute
    {
        public Type PropertyMetadataType { get; }
        public Type Type { get; }

        public FilePropertyDataTypeAttribute(Type type)
        {
            this.Type = type;
        }

        public FilePropertyDataTypeAttribute(Type propertyMetadataType, Type type)
        {
            this.PropertyMetadataType = propertyMetadataType;
            this.Type = type;
        }
    }

    public class FileDetails : BaseDictionary<string, object>
    {
        private FileInfo fileInfo;
        private ShellFile shellFile;
        private Dictionary<string, object> internalDictionary;

        public FileDetails(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
            shellFile = ShellFile.FromFilePath(fileInfo.FullName);
        }

        public Bitmap GetThumbnail()
        {
            Bitmap bitmap;

            try
            {
                bitmap = shellFile.Thumbnail.ExtraLargeBitmap;
            }
            catch
            {
                Thread.Sleep(1);

                bitmap = shellFile.Thumbnail.ExtraLargeBitmap;
            }

            return bitmap;
        }

        public override int Count
        {
            get
            {
                if (internalDictionary == null)
                {
                    internalDictionary = ((IEnumerable<KeyValuePair<string, object>>)this).ToDictionary(p => p.Key, p => p.Value);
                }

                return internalDictionary.Count;
            }
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override void Add(string key, object value)
        {
            throw new NotImplementedException();
        }

        public override bool ContainsKey(string key)
        {
            if (internalDictionary == null)
            {
                internalDictionary = this.Where(p => p.Value != null).DistinctByUtils(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
            }

            return internalDictionary.ContainsKey(key);
        }

        public override bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public override bool TryGetValue(string key, out object value)
        {
            if (internalDictionary == null)
            {
                internalDictionary = this.Where(p => p.Value != null).DistinctByUtils(p => p.Key).ToDictionary(p => p.Key, p => p.Value);
            }

            if (internalDictionary.ContainsKey(key))
            {
                value = internalDictionary[key];
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        public override IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            var headers = new List<string>();
            Folder folder;
            FolderItem item;

            try
            {
                var shell = new Shell();

                folder = shell.NameSpace(Path.GetDirectoryName(fileInfo.FullName));
                item = folder.Items().Cast<FolderItem>().SingleOrDefault(i => i.Name == fileInfo.Name);

                for (var x = 0; x < short.MaxValue; x++)
                {
                    var header = folder.GetDetailsOf(null, x);

                    if (String.IsNullOrEmpty(header))
                    {
                        break;
                    }

                    headers.Add(header);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            for (int x = 0; x < headers.Count; x++)
            {
                string header;
                string value;

                try
                {
                    header = headers[x];
                    value = folder.GetDetailsOf(item, x);
                }
                catch (Exception ex)
                {
                    throw;
                }

                yield return new KeyValuePair<string, object>(header, value);
            }
        }

        protected override void SetValue(string key, object value)
        {
            var parts = key.Split("/");

            if (parts.Length == 1)
            {
                var property = shellFile.Properties.System.GetProperty(key);
				var propertyValue = property.GetValue(shellFile.Properties.System);

                propertyValue.SetPropertyValue("AllowSetTruncatedValue", true);

                property = propertyValue.GetProperty("Value");

				switch (value)
                {
                    case Byte byteValue:
						property.SetValue(propertyValue , byteValue);
						break;
                    case Int16 int16Value:
						property.SetValue(propertyValue , int16Value);
						break;
                    case Int32 int32Value:
						property.SetValue(propertyValue , int32Value);
						break;
                    case UInt16 uint16Value:
						property.SetValue(propertyValue , uint16Value);
						break;
                    case UInt32 uint32Value:
						property.SetValue(propertyValue , uint32Value);
						break;
                    case Boolean boolValue:
						property.SetValue(propertyValue , boolValue);
						break;
                    case DateTime dateTimeValue:
						property.SetValue(propertyValue , dateTimeValue);
						break;
                    case String stringValue:
						property.SetValue(propertyValue , stringValue);
						break;
					case String[] stringArrayValue:
						property.SetValue(propertyValue , stringArrayValue);
						break;
				}
			}
            else if (parts.Length == 2)
            {
                var outerKey = parts[0];
                var innerKey = parts[1];
                var property = shellFile.Properties.System.GetProperty(outerKey);
                var propertyValue = property.GetValue(shellFile.Properties.System);

                property = propertyValue.GetProperty(innerKey);
                propertyValue = property.GetValue(propertyValue);

                property = propertyValue.GetProperty("Value");

                switch (value)
                {
                    case Byte byteValue:
                        property.SetValue(propertyValue, byteValue);
                        break;
                    case Int16 int16Value:
                        property.SetValue(propertyValue, int16Value);
                        break;
                    case Int32 int32Value:
                        property.SetValue(propertyValue, int32Value);
                        break;
                    case UInt16 uint16Value:
                        property.SetValue(propertyValue, uint16Value);
                        break;
                    case UInt32 uint32Value:
                        property.SetValue(propertyValue, uint32Value);
                        break;
                    case Boolean boolValue:
                        property.SetValue(propertyValue, boolValue);
                        break;
                    case DateTime dateTimeValue:
                        property.SetValue(propertyValue, dateTimeValue);
                        break;
                    case String stringValue:
                        property.SetValue(propertyValue, stringValue);
                        break;
                    case String[] stringArrayValue:
                        property.SetValue(propertyValue, stringArrayValue);
                        break;
                }
            }
        }
    }

    public static class ShellProperties
    {
        public enum GeneralProperties
        {
            [FilePropertyDataType(typeof(Nullable<int>))]
            AcquisitionID,
            [FilePropertyDataType(typeof(string))]
            ApplicationName,
            [FilePropertyDataType(typeof(String[]))]
            Author,
            [FilePropertyDataType(typeof(Nullable<ulong>))]
            Capacity,
            [FilePropertyDataType(typeof(String[]))]
            Category,
            [FilePropertyDataType(typeof(string))]
            Comment,
            [FilePropertyDataType(typeof(string))]
            Company,
            [FilePropertyDataType(typeof(string))]
            ComputerName,
            [FilePropertyDataType(typeof(IntPtr[]))]
            ContainedItems,
            [FilePropertyDataType(typeof(string))]
            ContentStatus,
            [FilePropertyDataType(typeof(string))]
            ContentType,
            [FilePropertyDataType(typeof(string))]
            Copyright,
            [FilePropertyDataType(typeof(Nullable<DateTime>))]
            DateAccessed,
            [FilePropertyDataType(typeof(Nullable<DateTime>))]
            DateAcquired,
            [FilePropertyDataType(typeof(Nullable<DateTime>))]
            DateArchived,
            [FilePropertyDataType(typeof(Nullable<DateTime>))]
            DateCompleted,
            [FilePropertyDataType(typeof(Nullable<DateTime>))]
            DateCreated,
            [FilePropertyDataType(typeof(Nullable<DateTime>))]
            DateImported,
            [FilePropertyDataType(typeof(Nullable<DateTime>))]
            DateModified,
            [FilePropertyDataType(typeof(Byte[]))]
            DescriptionID,
            [FilePropertyDataType(typeof(Nullable<DateTime>))]
            DueDate,
            [FilePropertyDataType(typeof(Nullable<DateTime>))]
            EndDate,
            [FilePropertyDataType(typeof(Nullable<ulong>))]
            FileAllocationSize,
            [FilePropertyDataType(typeof(Nullable<uint>))]
            FileAttributes,
            [FilePropertyDataType(typeof(Nullable<ulong>))]
            FileCount,
            [FilePropertyDataType(typeof(string))]
            FileDescription,
            [FilePropertyDataType(typeof(string))]
            FileExtension,
            [FilePropertyDataType(typeof(Nullable<ulong>))]
            FileFRN,
            [FilePropertyDataType(typeof(string))]
            FileName,
            [FilePropertyDataType(typeof(string))]
            FileOwner,
            [FilePropertyDataType(typeof(string))]
            FileVersion,
            [FilePropertyDataType(typeof(Byte[]))]
            FindData,
            [FilePropertyDataType(typeof(Nullable<ushort>))]
            FlagColor,
            [FilePropertyDataType(typeof(string))]
            FlagColorText,
            [FilePropertyDataType(typeof(Nullable<int>))]
            FlagStatus,
            [FilePropertyDataType(typeof(string))]
            FlagStatusText,
            [FilePropertyDataType(typeof(Nullable<ulong>))]
            FreeSpace,
            [FilePropertyDataType(typeof(string))]
            FullText,
            [FilePropertyDataType(typeof(string))]
            IdentityProperty,
            [FilePropertyDataType(typeof(String[]))]
            ImageParsingName,
            [FilePropertyDataType(typeof(Nullable<int>))]
            Importance,
            [FilePropertyDataType(typeof(string))]
            ImportanceText,
            [FilePropertyDataType(typeof(string))]
            InfoTipText,
            [FilePropertyDataType(typeof(string))]
            InternalName,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsAttachment,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsDefaultNonOwnerSaveLocation,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsDefaultSaveLocation,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsDeleted,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsEncrypted,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsFlagged,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsFlaggedComplete,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsIncomplete,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsLocationSupported,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsPinnedToNamespaceTree,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsRead,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsSearchOnlyItem,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsSendToTarget,
            [FilePropertyDataType(typeof(Nullable<bool>))]
            IsShared,
            [FilePropertyDataType(typeof(String[]))]
            ItemAuthors,
            [FilePropertyDataType(typeof(string))]
            ItemClassType,
            [FilePropertyDataType(typeof(Nullable<DateTime>))]
            ItemDate,
            [FilePropertyDataType(typeof(string))]
            ItemFolderNameDisplay,
            [FilePropertyDataType(typeof(string))]
            ItemFolderPathDisplay,
            [FilePropertyDataType(typeof(string))]
            ItemFolderPathDisplayNarrow,
            [FilePropertyDataType(typeof(string))]
            ItemName,
            [FilePropertyDataType(typeof(string))]
            ItemNameDisplay,
            [FilePropertyDataType(typeof(string))]
            ItemNamePrefix,
            [FilePropertyDataType(typeof(String[]))]
            ItemParticipants,
            [FilePropertyDataType(typeof(string))]
            ItemPathDisplay,
            [FilePropertyDataType(typeof(string))]
            ItemPathDisplayNarrow,
            [FilePropertyDataType(typeof(string))]
            ItemType,
            [FilePropertyDataType(typeof(string))]
            ItemTypeText,
            [FilePropertyDataType(typeof(string))]
            ItemUrl,
            [FilePropertyDataType(typeof(String[]))]
            Keywords,
            [FilePropertyDataType(typeof(String[]))]
            Kind,
            [FilePropertyDataType(typeof(string))]
            KindText,
            [FilePropertyDataType(typeof(string))]
            Language,
            [FilePropertyDataType(typeof(string))]
            MileageInformation,
            [FilePropertyDataType(typeof(string))]
            MIMEType,
            [FilePropertyDataType(typeof(Nullable<IntPtr>))]
            NamespaceClsid,
            [FilePropertyDataType(typeof(object))]
            Null,
            [FilePropertyDataType(typeof(Nullable<uint>))]
            OfflineAvailability,
            [FilePropertyDataType(typeof(Nullable<uint>))]
            OfflineStatus,
            [FilePropertyDataType(typeof(string))]
            OriginalFileName,
            [FilePropertyDataType(typeof(string))]
            OwnerSid,
            [FilePropertyDataType(typeof(string))]
            ParentalRating,
            [FilePropertyDataType(typeof(string))]
            ParentalRatingReason,
            [FilePropertyDataType(typeof(string))]
            ParentalRatingsOrganization,
            [FilePropertyDataType(typeof(object))]
            ParsingBindContext,
            [FilePropertyDataType(typeof(string))]
            ParsingName,
            [FilePropertyDataType(typeof(string))]
            ParsingPath,
            [FilePropertyDataType(typeof(Nullable<int>))]
            PerceivedType,
            [FilePropertyDataType(typeof(Nullable<uint>))]
            PercentFull,
            [FilePropertyDataType(typeof(Nullable<ushort>))]
            Priority,
            [FilePropertyDataType(typeof(string))]
            PriorityText,
            [FilePropertyDataType(typeof(string))]
            Project,
            [FilePropertyDataType(typeof(string))]
            ProviderItemID,
            [FilePropertyDataType(typeof(Nullable<uint>))]
            Rating,
            [FilePropertyDataType(typeof(string))]
            RatingText,
            [FilePropertyDataType(typeof(Nullable<ushort>))]
            Sensitivity,
            [FilePropertyDataType(typeof(string))]
            SensitivityText,
            [FilePropertyDataType(typeof(Nullable<uint>))]
            SFGAOFlags,
            [FilePropertyDataType(typeof(String[]))]
            SharedWith,
            [FilePropertyDataType(typeof(Nullable<uint>))]
            ShareUserRating,
            [FilePropertyDataType(typeof(Nullable<uint>))]
            SharingStatus,
            [FilePropertyDataType(typeof(Nullable<uint>))]
            SimpleRating,
            [FilePropertyDataType(typeof(Nullable<ulong>))]
            Size,
            [FilePropertyDataType(typeof(string))]
            SoftwareUsed,
            [FilePropertyDataType(typeof(string))]
            SourceItem,
            [FilePropertyDataType(typeof(Nullable<DateTime>))]
            StartDate,
            [FilePropertyDataType(typeof(string))]
            Status,
            [FilePropertyDataType(typeof(string))]
            Subject,
            [FilePropertyDataType(typeof(Nullable<IntPtr>))]
            Thumbnail,
            [FilePropertyDataType(typeof(Nullable<ulong>))]
            ThumbnailCacheId,
            [FilePropertyDataType(typeof(IStream))]
            ThumbnailStream,
            [FilePropertyDataType(typeof(string))]
            Title,
            [FilePropertyDataType(typeof(Nullable<ulong>))]
            TotalFileSize,
            [FilePropertyDataType(typeof(string))]
            Trademarks,
        }
        public enum AppUserModelProperties
        {
            [FilePropertyDataType(typeof(PropertySystemAppUserModel), typeof(Nullable<bool>))]
            ExcludeFromShowInNewInstall,
            [FilePropertyDataType(typeof(PropertySystemAppUserModel), typeof(string))]
            ID,
            [FilePropertyDataType(typeof(PropertySystemAppUserModel), typeof(Nullable<bool>))]
            IsDestinationListSeparator,
            [FilePropertyDataType(typeof(PropertySystemAppUserModel), typeof(Nullable<bool>))]
            PreventPinning,
            [FilePropertyDataType(typeof(PropertySystemAppUserModel), typeof(string))]
            RelaunchCommand,
            [FilePropertyDataType(typeof(PropertySystemAppUserModel), typeof(string))]
            RelaunchDisplayNameResource,
            [FilePropertyDataType(typeof(PropertySystemAppUserModel), typeof(string))]
            RelaunchIconResource,
        }
        public enum AudioProperties
        {
            [FilePropertyDataType(typeof(PropertySystemAudio), typeof(Nullable<uint>))]
            ChannelCount,
            [FilePropertyDataType(typeof(PropertySystemAudio), typeof(string))]
            Compression,
            [FilePropertyDataType(typeof(PropertySystemAudio), typeof(Nullable<uint>))]
            EncodingBitrate,
            [FilePropertyDataType(typeof(PropertySystemAudio), typeof(string))]
            Format,
            [FilePropertyDataType(typeof(PropertySystemAudio), typeof(Nullable<bool>))]
            IsVariableBitrate,
            [FilePropertyDataType(typeof(PropertySystemAudio), typeof(Nullable<uint>))]
            PeakValue,
            [FilePropertyDataType(typeof(PropertySystemAudio), typeof(Nullable<uint>))]
            SampleRate,
            [FilePropertyDataType(typeof(PropertySystemAudio), typeof(Nullable<uint>))]
            SampleSize,
            [FilePropertyDataType(typeof(PropertySystemAudio), typeof(string))]
            StreamName,
            [FilePropertyDataType(typeof(PropertySystemAudio), typeof(Nullable<ushort>))]
            StreamNumber,
        }
        public enum CalendarProperties
        {
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(string))]
            Duration,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(Nullable<bool>))]
            IsOnline,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(Nullable<bool>))]
            IsRecurring,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(string))]
            Location,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(String[]))]
            OptionalAttendeeAddresses,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(String[]))]
            OptionalAttendeeNames,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(string))]
            OrganizerAddress,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(string))]
            OrganizerName,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(Nullable<DateTime>))]
            ReminderTime,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(String[]))]
            RequiredAttendeeAddresses,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(String[]))]
            RequiredAttendeeNames,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(String[]))]
            Resources,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(Nullable<ushort>))]
            ResponseStatus,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(Nullable<ushort>))]
            ShowTimeAs,
            [FilePropertyDataType(typeof(PropertySystemCalendar), typeof(string))]
            ShowTimeAsText,
        }
        public enum CommunicationProperties
        {
            [FilePropertyDataType(typeof(PropertySystemCommunication), typeof(string))]
            AccountName,
            [FilePropertyDataType(typeof(PropertySystemCommunication), typeof(Nullable<DateTime>))]
            DateItemExpires,
            [FilePropertyDataType(typeof(PropertySystemCommunication), typeof(Nullable<int>))]
            FollowUpIconIndex,
            [FilePropertyDataType(typeof(PropertySystemCommunication), typeof(Nullable<bool>))]
            HeaderItem,
            [FilePropertyDataType(typeof(PropertySystemCommunication), typeof(string))]
            PolicyTag,
            [FilePropertyDataType(typeof(PropertySystemCommunication), typeof(Nullable<int>))]
            SecurityFlags,
            [FilePropertyDataType(typeof(PropertySystemCommunication), typeof(string))]
            Suffix,
            [FilePropertyDataType(typeof(PropertySystemCommunication), typeof(Nullable<ushort>))]
            TaskStatus,
            [FilePropertyDataType(typeof(PropertySystemCommunication), typeof(string))]
            TaskStatusText,
        }
        public enum ComputerProperties
        {
            [FilePropertyDataType(typeof(PropertySystemComputer), typeof(UInt64[]))]
            DecoratedFreeSpace,
        }
        public enum ContactProperties
        {
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(Nullable<DateTime>))]
            Anniversary,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            AssistantName,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            AssistantTelephone,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(Nullable<DateTime>))]
            Birthday,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            BusinessAddress,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            BusinessAddressCity,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            BusinessAddressCountry,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            BusinessAddressPostalCode,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            BusinessAddressPostOfficeBox,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            BusinessAddressState,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            BusinessAddressStreet,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            BusinessFaxNumber,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            BusinessHomepage,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            BusinessTelephone,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            CallbackTelephone,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            CarTelephone,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(String[]))]
            Children,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            CompanyMainTelephone,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            Department,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            EmailAddress,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            EmailAddress2,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            EmailAddress3,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(String[]))]
            EmailAddresses,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            EmailName,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            FileAsName,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            FirstName,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            FullName,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            Gender,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(Nullable<ushort>))]
            GenderValue,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(String[]))]
            Hobbies,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            HomeAddress,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            HomeAddressCity,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            HomeAddressCountry,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            HomeAddressPostalCode,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            HomeAddressPostOfficeBox,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            HomeAddressState,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            HomeAddressStreet,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            HomeFaxNumber,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            HomeTelephone,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(String[]))]
            IMAddress,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            Initials,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            JobTitle,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            Label,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            LastName,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            MailingAddress,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            MiddleName,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            MobileTelephone,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            Nickname,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            OfficeLocation,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            OtherAddress,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            OtherAddressCity,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            OtherAddressCountry,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            OtherAddressPostalCode,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            OtherAddressPostOfficeBox,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            OtherAddressState,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            OtherAddressStreet,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            PagerTelephone,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            PersonalTitle,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            PrimaryAddressCity,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            PrimaryAddressCountry,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            PrimaryAddressPostalCode,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            PrimaryAddressPostOfficeBox,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            PrimaryAddressState,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            PrimaryAddressStreet,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            PrimaryEmailAddress,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            PrimaryTelephone,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            Profession,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            SpouseName,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            Suffix,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            TelexNumber,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            TTYTDDTelephone,
            [FilePropertyDataType(typeof(PropertySystemContact), typeof(string))]
            Webpage,
        }
        public enum DeviceProperties
        {
            [FilePropertyDataType(typeof(PropertySystemDevice), typeof(string))]
            PrinterUrl,
        }
        public enum DeviceInterfaceProperties
        {
            [FilePropertyDataType(typeof(PropertySystemDeviceInterface), typeof(string))]
            PrinterDriverDirectory,
            [FilePropertyDataType(typeof(PropertySystemDeviceInterface), typeof(string))]
            PrinterDriverName,
            [FilePropertyDataType(typeof(PropertySystemDeviceInterface), typeof(string))]
            PrinterName,
            [FilePropertyDataType(typeof(PropertySystemDeviceInterface), typeof(string))]
            PrinterPortName,
        }
        public enum DevicesProperties
        {
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<byte>))]
            BatteryLife,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<byte>))]
            BatteryPlusCharging,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            BatteryPlusChargingText,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(String[]))]
            Category,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(String[]))]
            CategoryGroup,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(String[]))]
            CategoryPlural,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<byte>))]
            ChargingState,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<bool>))]
            Connected,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<IntPtr>))]
            ContainerId,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            DefaultTooltip,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            DeviceDescription1,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            DeviceDescription2,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(String[]))]
            DiscoveryMethod,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            FriendlyName,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(String[]))]
            FunctionPaths,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(String[]))]
            InterfacePaths,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<bool>))]
            IsDefault,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<bool>))]
            IsNetworkConnected,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<bool>))]
            IsShared,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<bool>))]
            IsSoftwareInstalling,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<bool>))]
            LaunchDeviceStageFromExplorer,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<bool>))]
            LocalMachine,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            Manufacturer,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<byte>))]
            MissedCalls,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            ModelName,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            ModelNumber,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            NetworkedTooltip,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            NetworkName,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            NetworkType,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<ushort>))]
            NewPictures,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            Notification,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<IntPtr>))]
            NotificationStore,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<bool>))]
            NotWorkingProperly,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<bool>))]
            Paired,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            PrimaryCategory,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<byte>))]
            Roaming,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<bool>))]
            SafeRemovalRequired,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            SharedTooltip,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<byte>))]
            SignalStrength,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            Status1,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(string))]
            Status2,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<ulong>))]
            StorageCapacity,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<ulong>))]
            StorageFreeSpace,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<uint>))]
            StorageFreeSpacePercent,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<byte>))]
            TextMessages,
            [FilePropertyDataType(typeof(PropertySystemDevices), typeof(Nullable<byte>))]
            Voicemail,
        }
        public enum DocumentProperties
        {
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<int>))]
            ByteCount,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<int>))]
            CharacterCount,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(string))]
            ClientID,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(String[]))]
            Contributor,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<DateTime>))]
            DateCreated,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<DateTime>))]
            DatePrinted,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<DateTime>))]
            DateSaved,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(string))]
            Division,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(string))]
            DocumentID,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<int>))]
            HiddenSlideCount,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(string))]
            LastAuthor,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<int>))]
            LineCount,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(string))]
            Manager,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<int>))]
            MultimediaClipCount,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<int>))]
            NoteCount,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<int>))]
            PageCount,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<int>))]
            ParagraphCount,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(string))]
            PresentationFormat,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(string))]
            RevisionNumber,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<int>))]
            Security,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<int>))]
            SlideCount,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(string))]
            Template,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<ulong>))]
            TotalEditingTime,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(string))]
            Version,
            [FilePropertyDataType(typeof(PropertySystemDocument), typeof(Nullable<int>))]
            WordCount,
        }
        public enum DRMProperties
        {
            [FilePropertyDataType(typeof(PropertySystemDRM), typeof(Nullable<DateTime>))]
            DatePlayExpires,
            [FilePropertyDataType(typeof(PropertySystemDRM), typeof(Nullable<DateTime>))]
            DatePlayStarts,
            [FilePropertyDataType(typeof(PropertySystemDRM), typeof(string))]
            Description,
            [FilePropertyDataType(typeof(PropertySystemDRM), typeof(Nullable<bool>))]
            IsProtected,
            [FilePropertyDataType(typeof(PropertySystemDRM), typeof(Nullable<uint>))]
            PlayCount,
        }
        public enum GPSProperties
        {
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<double>))]
            Altitude,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            AltitudeDenominator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            AltitudeNumerator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<byte>))]
            AltitudeRef,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            AreaInformation,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<DateTime>))]
            Date,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<double>))]
            DestinationBearing,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            DestinationBearingDenominator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            DestinationBearingNumerator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            DestinationBearingRef,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<double>))]
            DestinationDistance,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            DestinationDistanceDenominator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            DestinationDistanceNumerator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            DestinationDistanceRef,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Double[]))]
            DestinationLatitude,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(UInt32[]))]
            DestinationLatitudeDenominator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(UInt32[]))]
            DestinationLatitudeNumerator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            DestinationLatitudeRef,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Double[]))]
            DestinationLongitude,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(UInt32[]))]
            DestinationLongitudeDenominator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(UInt32[]))]
            DestinationLongitudeNumerator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            DestinationLongitudeRef,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<ushort>))]
            Differential,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<double>))]
            DOP,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            DOPDenominator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            DOPNumerator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<double>))]
            ImageDirection,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            ImageDirectionDenominator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            ImageDirectionNumerator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            ImageDirectionRef,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Double[]))]
            Latitude,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(UInt32[]))]
            LatitudeDenominator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(UInt32[]))]
            LatitudeNumerator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            LatitudeRef,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Double[]))]
            Longitude,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(UInt32[]))]
            LongitudeDenominator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(UInt32[]))]
            LongitudeNumerator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            LongitudeRef,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            MapDatum,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            MeasureMode,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            ProcessingMethod,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            Satellites,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<double>))]
            Speed,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            SpeedDenominator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            SpeedNumerator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            SpeedRef,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            Status,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<double>))]
            Track,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            TrackDenominator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Nullable<uint>))]
            TrackNumerator,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(string))]
            TrackRef,
            [FilePropertyDataType(typeof(PropertySystemGPS), typeof(Byte[]))]
            VersionID,
        }
        public enum IdentityProperties
        {
            [FilePropertyDataType(typeof(PropertySystemIdentity), typeof(Byte[]))]
            Blob,
            [FilePropertyDataType(typeof(PropertySystemIdentity), typeof(string))]
            DisplayName,
            [FilePropertyDataType(typeof(PropertySystemIdentity), typeof(Nullable<bool>))]
            IsMeIdentity,
            [FilePropertyDataType(typeof(PropertySystemIdentity), typeof(string))]
            PrimaryEmailAddress,
            [FilePropertyDataType(typeof(PropertySystemIdentity), typeof(Nullable<IntPtr>))]
            ProviderID,
            [FilePropertyDataType(typeof(PropertySystemIdentity), typeof(string))]
            UniqueID,
            [FilePropertyDataType(typeof(PropertySystemIdentity), typeof(string))]
            UserName,
        }
        public enum IdentityProviderProperties
        {
            [FilePropertyDataType(typeof(PropertySystemIdentityProvider), typeof(string))]
            Name,
            [FilePropertyDataType(typeof(PropertySystemIdentityProvider), typeof(string))]
            Picture,
        }
        public enum ImageProperties
        {
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(Nullable<uint>))]
            BitDepth,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(Nullable<ushort>))]
            ColorSpace,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(Nullable<double>))]
            CompressedBitsPerPixel,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(Nullable<uint>))]
            CompressedBitsPerPixelDenominator,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(Nullable<uint>))]
            CompressedBitsPerPixelNumerator,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(Nullable<ushort>))]
            Compression,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(string))]
            CompressionText,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(string))]
            Dimensions,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(Nullable<double>))]
            HorizontalResolution,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(Nullable<uint>))]
            HorizontalSize,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(string))]
            ImageID,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(Nullable<short>))]
            ResolutionUnit,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(Nullable<double>))]
            VerticalResolution,
            [FilePropertyDataType(typeof(PropertySystemImage), typeof(Nullable<uint>))]
            VerticalSize,
        }
        public enum JournalProperties
        {
            [FilePropertyDataType(typeof(PropertySystemJournal), typeof(String[]))]
            Contacts,
            [FilePropertyDataType(typeof(PropertySystemJournal), typeof(string))]
            EntryType,
        }
        public enum LayoutPatternProperties
        {
            [FilePropertyDataType(typeof(PropertySystemLayoutPattern), typeof(string))]
            ContentViewModeForBrowse,
            [FilePropertyDataType(typeof(PropertySystemLayoutPattern), typeof(string))]
            ContentViewModeForSearch,
        }
        public enum LinkProperties
        {
            [FilePropertyDataType(typeof(PropertySystemLink), typeof(string))]
            Arguments,
            [FilePropertyDataType(typeof(PropertySystemLink), typeof(string))]
            Comment,
            [FilePropertyDataType(typeof(PropertySystemLink), typeof(Nullable<DateTime>))]
            DateVisited,
            [FilePropertyDataType(typeof(PropertySystemLink), typeof(string))]
            Description,
            [FilePropertyDataType(typeof(PropertySystemLink), typeof(Nullable<int>))]
            Status,
            [FilePropertyDataType(typeof(PropertySystemLink), typeof(String[]))]
            TargetExtension,
            [FilePropertyDataType(typeof(PropertySystemLink), typeof(string))]
            TargetParsingPath,
            [FilePropertyDataType(typeof(PropertySystemLink), typeof(Nullable<uint>))]
            TargetSFGAOFlags,
            [FilePropertyDataType(typeof(PropertySystemLink), typeof(String[]))]
            TargetSFGAOFlagsStrings,
            [FilePropertyDataType(typeof(PropertySystemLink), typeof(string))]
            TargetUrl,
        }
        public enum MediaProperties
        {
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            AuthorUrl,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(Nullable<uint>))]
            AverageLevel,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            ClassPrimaryID,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            ClassSecondaryID,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            CollectionGroupID,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            CollectionID,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            ContentDistributor,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            ContentID,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            CreatorApplication,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            CreatorApplicationVersion,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(Nullable<DateTime>))]
            DateEncoded,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            DateReleased,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(Nullable<ulong>))]
            Duration,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            DVDID,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            EncodedBy,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            EncodingSettings,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(Nullable<uint>))]
            FrameCount,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            MCDI,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            MetadataContentProvider,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(String[]))]
            Producer,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            PromotionUrl,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            ProtectionType,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            ProviderRating,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            ProviderStyle,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            Publisher,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            SubscriptionContentId,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            Subtitle,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            UniqueFileIdentifier,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            UserNoAutoInfo,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(string))]
            UserWebUrl,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(String[]))]
            Writer,
            [FilePropertyDataType(typeof(PropertySystemMedia), typeof(Nullable<uint>))]
            Year,
        }
        public enum MessageProperties
        {
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(string))]
            AttachmentContents,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(String[]))]
            AttachmentNames,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(String[]))]
            BccAddress,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(String[]))]
            BccName,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(String[]))]
            CcAddress,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(String[]))]
            CcName,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(string))]
            ConversationID,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(Byte[]))]
            ConversationIndex,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(Nullable<DateTime>))]
            DateReceived,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(Nullable<DateTime>))]
            DateSent,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(Nullable<int>))]
            Flags,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(String[]))]
            FromAddress,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(String[]))]
            FromName,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(Nullable<bool>))]
            HasAttachments,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(Nullable<int>))]
            IsFwdOrReply,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(string))]
            MessageClass,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(Nullable<bool>))]
            ProofInProgress,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(string))]
            SenderAddress,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(string))]
            SenderName,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(string))]
            Store,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(String[]))]
            ToAddress,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(Nullable<int>))]
            ToDoFlags,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(string))]
            ToDoTitle,
            [FilePropertyDataType(typeof(PropertySystemMessage), typeof(String[]))]
            ToName,
        }
        public enum MusicProperties
        {
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(string))]
            AlbumArtist,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(string))]
            AlbumID,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(string))]
            AlbumTitle,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(String[]))]
            Artist,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(string))]
            BeatsPerMinute,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(String[]))]
            Composer,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(String[]))]
            Conductor,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(string))]
            ContentGroupDescription,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(string))]
            DisplayArtist,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(String[]))]
            Genre,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(string))]
            InitialKey,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(Nullable<bool>))]
            IsCompilation,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(string))]
            Lyrics,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(string))]
            Mood,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(string))]
            PartOfSet,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(string))]
            Period,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(Byte[]))]
            SynchronizedLyrics,
            [FilePropertyDataType(typeof(PropertySystemMusic), typeof(Nullable<uint>))]
            TrackNumber,
        }
        public enum NoteProperties
        {
            [FilePropertyDataType(typeof(PropertySystemNote), typeof(Nullable<ushort>))]
            Color,
            [FilePropertyDataType(typeof(PropertySystemNote), typeof(string))]
            ColorText,
        }
        public enum PhotoProperties
        {
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            Aperture,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            ApertureDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            ApertureNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            Brightness,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            BrightnessDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            BrightnessNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            CameraManufacturer,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            CameraModel,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            CameraSerialNumber,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            Contrast,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            ContrastText,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<DateTime>))]
            DateTaken,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            DigitalZoom,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            DigitalZoomDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            DigitalZoomNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(String[]))]
            Event,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            EXIFVersion,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            ExposureBias,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<int>))]
            ExposureBiasDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<int>))]
            ExposureBiasNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            ExposureIndex,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            ExposureIndexDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            ExposureIndexNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            ExposureProgram,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            ExposureProgramText,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            ExposureTime,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            ExposureTimeDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            ExposureTimeNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<byte>))]
            Flash,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            FlashEnergy,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            FlashEnergyDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            FlashEnergyNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            FlashManufacturer,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            FlashModel,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            FlashText,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            FNumber,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            FNumberDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            FNumberNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            FocalLength,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            FocalLengthDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<ushort>))]
            FocalLengthInFilm,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            FocalLengthNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            FocalPlaneXResolution,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            FocalPlaneXResolutionDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            FocalPlaneXResolutionNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            FocalPlaneYResolution,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            FocalPlaneYResolutionDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            FocalPlaneYResolutionNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            GainControl,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            GainControlDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            GainControlNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            GainControlText,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<ushort>))]
            ISOSpeed,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            LensManufacturer,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            LensModel,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            LightSource,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Byte[]))]
            MakerNote,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<ulong>))]
            MakerNoteOffset,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            MaxAperture,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            MaxApertureDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            MaxApertureNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<ushort>))]
            MeteringMode,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            MeteringModeText,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<ushort>))]
            Orientation,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            OrientationText,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(String[]))]
            PeopleNames,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<ushort>))]
            PhotometricInterpretation,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            PhotometricInterpretationText,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            ProgramMode,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            ProgramModeText,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            RelatedSoundFile,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            Saturation,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            SaturationText,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            Sharpness,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            SharpnessText,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            ShutterSpeed,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<int>))]
            ShutterSpeedDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<int>))]
            ShutterSpeedNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<double>))]
            SubjectDistance,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            SubjectDistanceDenominator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            SubjectDistanceNumerator,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(String[]))]
            TagViewAggregate,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<bool>))]
            TranscodedForSync,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(Nullable<uint>))]
            WhiteBalance,
            [FilePropertyDataType(typeof(PropertySystemPhoto), typeof(string))]
            WhiteBalanceText,
        }
        public enum PropGroupProperties
        {
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Advanced,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Audio,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Calendar,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Camera,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Contact,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Content,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Description,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            FileSystem,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            General,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            GPS,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Image,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Media,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            MediaAdvanced,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Message,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Music,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Origin,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            PhotoAdvanced,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            RecordedTV,
            [FilePropertyDataType(typeof(PropertySystemPropGroup), typeof(object))]
            Video,
        }
        public enum PropListProperties
        {
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            ConflictPrompt,
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            ContentViewModeForBrowse,
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            ContentViewModeForSearch,
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            ExtendedTileInfo,
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            FileOperationPrompt,
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            FullDetails,
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            InfoTip,
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            NonPersonal,
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            PreviewDetails,
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            PreviewTitle,
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            QuickTip,
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            TileInfo,
            [FilePropertyDataType(typeof(PropertySystemPropList), typeof(string))]
            XPDetailsPanel,
        }
        public enum RecordedTVProperties
        {
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(Nullable<uint>))]
            ChannelNumber,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(string))]
            Credits,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(Nullable<DateTime>))]
            DateContentExpires,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(string))]
            EpisodeName,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(Nullable<bool>))]
            IsATSCContent,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(Nullable<bool>))]
            IsClosedCaptioningAvailable,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(Nullable<bool>))]
            IsDTVContent,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(Nullable<bool>))]
            IsHDContent,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(Nullable<bool>))]
            IsRepeatBroadcast,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(Nullable<bool>))]
            IsSAP,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(string))]
            NetworkAffiliation,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(Nullable<DateTime>))]
            OriginalBroadcastDate,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(string))]
            ProgramDescription,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(Nullable<DateTime>))]
            RecordingTime,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(string))]
            StationCallSign,
            [FilePropertyDataType(typeof(PropertySystemRecordedTV), typeof(string))]
            StationName,
        }
        public enum SearchProperties
        {
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(string))]
            AutoSummary,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(string))]
            ContainerHash,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(string))]
            Contents,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(Nullable<int>))]
            EntryID,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(Byte[]))]
            ExtendedProperties,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(Nullable<DateTime>))]
            GatherTime,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(Nullable<int>))]
            HitCount,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(Nullable<bool>))]
            IsClosedDirectory,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(Nullable<bool>))]
            IsFullyContained,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(string))]
            QueryFocusedSummary,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(string))]
            QueryFocusedSummaryWithFallback,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(Nullable<int>))]
            Rank,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(string))]
            Store,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(string))]
            UrlToIndex,
            [FilePropertyDataType(typeof(PropertySystemSearch), typeof(object))]
            UrlToIndexWithModificationTime,
        }
        public enum ShellSpecificProperties
        {
            [FilePropertyDataType(typeof(PropertySystemShell), typeof(string))]
            OmitFromView,
            [FilePropertyDataType(typeof(PropertySystemShell), typeof(String[]))]
            SFGAOFlagsStrings,
        }
        public enum SoftwareProperties
        {
            [FilePropertyDataType(typeof(PropertySystemSoftware), typeof(Nullable<DateTime>))]
            DateLastUsed,
            [FilePropertyDataType(typeof(PropertySystemSoftware), typeof(string))]
            ProductName,
        }
        public enum SyncProperties
        {
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(string))]
            Comments,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(string))]
            ConflictDescription,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(string))]
            ConflictFirstLocation,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(string))]
            ConflictSecondLocation,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(Nullable<IntPtr>))]
            HandlerCollectionID,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(string))]
            HandlerID,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(string))]
            HandlerName,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(Nullable<uint>))]
            HandlerType,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(string))]
            HandlerTypeLabel,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(string))]
            ItemID,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(string))]
            ItemName,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(Nullable<uint>))]
            ProgressPercentage,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(Nullable<uint>))]
            State,
            [FilePropertyDataType(typeof(PropertySystemSync), typeof(string))]
            Status,
        }
        public enum TaskProperties
        {
            [FilePropertyDataType(typeof(PropertySystemTask), typeof(string))]
            BillingInformation,
            [FilePropertyDataType(typeof(PropertySystemTask), typeof(string))]
            CompletionStatus,
            [FilePropertyDataType(typeof(PropertySystemTask), typeof(string))]
            Owner,
        }
        public enum VideoProperties
        {
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(string))]
            Compression,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(String[]))]
            Director,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(Nullable<uint>))]
            EncodingBitrate,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(Nullable<uint>))]
            FourCC,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(Nullable<uint>))]
            FrameHeight,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(Nullable<uint>))]
            FrameRate,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(Nullable<uint>))]
            FrameWidth,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(Nullable<uint>))]
            HorizontalAspectRatio,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(Nullable<uint>))]
            SampleSize,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(string))]
            StreamName,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(Nullable<ushort>))]
            StreamNumber,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(Nullable<uint>))]
            TotalBitrate,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(Nullable<bool>))]
            TranscodedForSync,
            [FilePropertyDataType(typeof(PropertySystemVideo), typeof(Nullable<uint>))]
            VerticalAspectRatio,
        }
        public enum VolumeProperties
        {
            [FilePropertyDataType(typeof(PropertySystemVolume), typeof(string))]
            FileSystem,
            [FilePropertyDataType(typeof(PropertySystemVolume), typeof(Nullable<bool>))]
            IsMappedDrive,
            [FilePropertyDataType(typeof(PropertySystemVolume), typeof(Nullable<bool>))]
            IsRoot,
        }
	}
}