using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class FileDetailExtensions
    {
		public static void SetGeneralAcquisitionIDProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["AcquisitionID"] = value;
		}

		public static void SetGeneralApplicationNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ApplicationName"] = value;
		}

		public static void SetGeneralAuthorProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Author"] = value;
		}

		public static void SetGeneralAuthorProperty(this FileDetails fileDetails, String value)
		{
			fileDetails["Author"] = new[] { value };
		}

		public static void SetGeneralCapacityProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["Capacity"] = value;
		}

		public static void SetGeneralCategoryProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Category"] = value;
		}

		public static string[] GetGeneralCategoryProperty(this FileDetails fileDetails)
		{
			return (string[]) fileDetails["Category"];
		}

		public static void SetGeneralCommentProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Comment"] = value;
		}

		public static void SetGeneralCompanyProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Company"] = value;
		}

		public static void SetGeneralComputerNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ComputerName"] = value;
		}

		public static void SetGeneralContainedItemsProperty(this FileDetails fileDetails, IntPtr[] value)
		{
			fileDetails["ContainedItems"] = value;
		}

		public static void SetGeneralContentStatusProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ContentStatus"] = value;
		}

		public static void SetGeneralContentTypeProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ContentType"] = value;
		}

		public static void SetGeneralCopyrightProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Copyright"] = value;
		}

		public static void SetGeneralDateAccessedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["DateAccessed"] = value;
		}

		public static void SetGeneralDateAcquiredProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["DateAcquired"] = value;
		}

		public static void SetGeneralDateArchivedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["DateArchived"] = value;
		}

		public static void SetGeneralDateCompletedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["DateCompleted"] = value;
		}

		public static void SetGeneralDateCreatedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["DateCreated"] = value;
		}

		public static void SetGeneralDateImportedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["DateImported"] = value;
		}

		public static void SetGeneralDateModifiedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["DateModified"] = value;
		}

		public static void SetGeneralDescriptionIDProperty(this FileDetails fileDetails, Byte[] value)
		{
			fileDetails["DescriptionID"] = value;
		}

		public static void SetGeneralDueDateProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["DueDate"] = value;
		}

		public static void SetGeneralEndDateProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["EndDate"] = value;
		}

		public static void SetGeneralFileAllocationSizeProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["FileAllocationSize"] = value;
		}

		public static void SetGeneralFileAttributesProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["FileAttributes"] = value;
		}

		public static void SetGeneralFileCountProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["FileCount"] = value;
		}

		public static void SetGeneralFileDescriptionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["FileDescription"] = value;
		}

		public static void SetGeneralFileExtensionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["FileExtension"] = value;
		}

		public static void SetGeneralFileFRNProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["FileFRN"] = value;
		}

		public static void SetGeneralFileNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["FileName"] = value;
		}

		public static void SetGeneralFileOwnerProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["FileOwner"] = value;
		}

		public static void SetGeneralFileVersionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["FileVersion"] = value;
		}

		public static void SetGeneralFindDataProperty(this FileDetails fileDetails, Byte[] value)
		{
			fileDetails["FindData"] = value;
		}

		public static void SetGeneralFlagColorProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["FlagColor"] = value;
		}

		public static void SetGeneralFlagColorTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["FlagColorText"] = value;
		}

		public static void SetGeneralFlagStatusProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["FlagStatus"] = value;
		}

		public static void SetGeneralFlagStatusTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["FlagStatusText"] = value;
		}

		public static void SetGeneralFreeSpaceProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["FreeSpace"] = value;
		}

		public static void SetGeneralFullTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["FullText"] = value;
		}

		public static void SetGeneralIdentityPropertyProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["IdentityProperty"] = value;
		}

		public static void SetGeneralImageParsingNameProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["ImageParsingName"] = value;
		}

		public static void SetGeneralImportanceProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Importance"] = value;
		}

		public static void SetGeneralImportanceTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ImportanceText"] = value;
		}

		public static void SetGeneralInfoTipTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["InfoTipText"] = value;
		}

		public static void SetGeneralInternalNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["InternalName"] = value;
		}

		public static void SetGeneralIsAttachmentProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsAttachment"] = value;
		}

		public static void SetGeneralIsDefaultNonOwnerSaveLocationProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsDefaultNonOwnerSaveLocation"] = value;
		}

		public static void SetGeneralIsDefaultSaveLocationProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsDefaultSaveLocation"] = value;
		}

		public static void SetGeneralIsDeletedProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsDeleted"] = value;
		}

		public static void SetGeneralIsEncryptedProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsEncrypted"] = value;
		}

		public static void SetGeneralIsFlaggedProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsFlagged"] = value;
		}

		public static void SetGeneralIsFlaggedCompleteProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsFlaggedComplete"] = value;
		}

		public static void SetGeneralIsIncompleteProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsIncomplete"] = value;
		}

		public static void SetGeneralIsLocationSupportedProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsLocationSupported"] = value;
		}

		public static void SetGeneralIsPinnedToNamespaceTreeProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsPinnedToNamespaceTree"] = value;
		}

		public static void SetGeneralIsReadProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsRead"] = value;
		}

		public static void SetGeneralIsSearchOnlyItemProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsSearchOnlyItem"] = value;
		}

		public static void SetGeneralIsSendToTargetProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsSendToTarget"] = value;
		}

		public static void SetGeneralIsSharedProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["IsShared"] = value;
		}

		public static void SetGeneralItemAuthorsProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["ItemAuthors"] = value;
		}

		public static void SetGeneralItemClassTypeProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ItemClassType"] = value;
		}

		public static void SetGeneralItemDateProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["ItemDate"] = value;
		}

		public static void SetGeneralItemFolderNameDisplayProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ItemFolderNameDisplay"] = value;
		}

		public static void SetGeneralItemFolderPathDisplayProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ItemFolderPathDisplay"] = value;
		}

		public static void SetGeneralItemFolderPathDisplayNarrowProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ItemFolderPathDisplayNarrow"] = value;
		}

		public static void SetGeneralItemNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ItemName"] = value;
		}

		public static void SetGeneralItemNameDisplayProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ItemNameDisplay"] = value;
		}

		public static void SetGeneralItemNamePrefixProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ItemNamePrefix"] = value;
		}

		public static void SetGeneralItemParticipantsProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["ItemParticipants"] = value;
		}

		public static void SetGeneralItemPathDisplayProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ItemPathDisplay"] = value;
		}

		public static void SetGeneralItemPathDisplayNarrowProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ItemPathDisplayNarrow"] = value;
		}

		public static void SetGeneralItemTypeProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ItemType"] = value;
		}

		public static void SetGeneralItemTypeTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ItemTypeText"] = value;
		}

		public static void SetGeneralItemUrlProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ItemUrl"] = value;
		}

		public static void SetGeneralKeywordsProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Keywords"] = value;
		}

		public static void SetGeneralKindProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Kind"] = value;
		}

		public static void SetGeneralKindTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["KindText"] = value;
		}

		public static void SetGeneralLanguageProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Language"] = value;
		}

		public static void SetGeneralMileageInformationProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["MileageInformation"] = value;
		}

		public static void SetGeneralMIMETypeProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["MIMEType"] = value;
		}

		public static void SetGeneralNamespaceClsidProperty(this FileDetails fileDetails, Nullable<IntPtr> value)
		{
			fileDetails["NamespaceClsid"] = value;
		}

		public static void SetGeneralNullProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["Null"] = value;
		}

		public static void SetGeneralOfflineAvailabilityProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["OfflineAvailability"] = value;
		}

		public static void SetGeneralOfflineStatusProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["OfflineStatus"] = value;
		}

		public static void SetGeneralOriginalFileNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["OriginalFileName"] = value;
		}

		public static void SetGeneralOwnerSidProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["OwnerSid"] = value;
		}

		public static void SetGeneralParentalRatingProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ParentalRating"] = value;
		}

		public static void SetGeneralParentalRatingReasonProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ParentalRatingReason"] = value;
		}

		public static void SetGeneralParentalRatingsOrganizationProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ParentalRatingsOrganization"] = value;
		}

		public static void SetGeneralParsingBindContextProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["ParsingBindContext"] = value;
		}

		public static void SetGeneralParsingNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ParsingName"] = value;
		}

		public static void SetGeneralParsingPathProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ParsingPath"] = value;
		}

		public static void SetGeneralPerceivedTypeProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["PerceivedType"] = value;
		}

		public static void SetGeneralPercentFullProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["PercentFull"] = value;
		}

		public static void SetGeneralPriorityProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Priority"] = value;
		}

		public static void SetGeneralPriorityTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PriorityText"] = value;
		}

		public static void SetGeneralProjectProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Project"] = value;
		}

		public static void SetGeneralProviderItemIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["ProviderItemID"] = value;
		}

		public static void SetGeneralRatingProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Rating"] = value;
		}

		public static void SetGeneralRatingTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["RatingText"] = value;
		}

		public static void SetGeneralSensitivityProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Sensitivity"] = value;
		}

		public static void SetGeneralSensitivityTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["SensitivityText"] = value;
		}

		public static void SetGeneralSFGAOFlagsProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["SFGAOFlags"] = value;
		}

		public static void SetGeneralSharedWithProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["SharedWith"] = value;
		}

		public static void SetGeneralShareUserRatingProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["ShareUserRating"] = value;
		}

		public static void SetGeneralSharingStatusProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["SharingStatus"] = value;
		}

		public static void SetGeneralSimpleRatingProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["SimpleRating"] = value;
		}

		public static void SetGeneralSizeProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["Size"] = value;
		}

		public static void SetGeneralSoftwareUsedProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["SoftwareUsed"] = value;
		}

		public static void SetGeneralSourceItemProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["SourceItem"] = value;
		}

		public static void SetGeneralStartDateProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["StartDate"] = value;
		}

		public static void SetGeneralStatusProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Status"] = value;
		}

		public static void SetGeneralSubjectProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Subject"] = value;
		}

		public static void SetGeneralThumbnailProperty(this FileDetails fileDetails, Nullable<IntPtr> value)
		{
			fileDetails["Thumbnail"] = value;
		}

		public static void SetGeneralThumbnailCacheIdProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["ThumbnailCacheId"] = value;
		}

		public static void SetGeneralThumbnailStreamProperty(this FileDetails fileDetails, IStream value)
		{
			fileDetails["ThumbnailStream"] = value;
		}

		public static void SetGeneralTitleProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Title"] = value;
		}

		public static void SetGeneralTotalFileSizeProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["TotalFileSize"] = value;
		}

		public static void SetGeneralTrademarksProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Trademarks"] = value;
		}

		public static void SetAppUserModelExcludeFromShowInNewInstallProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["AppUserModel/ExcludeFromShowInNewInstall"] = value;
		}

		public static void SetAppUserModelIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["AppUserModel/ID"] = value;
		}

		public static void SetAppUserModelIsDestinationListSeparatorProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["AppUserModel/IsDestinationListSeparator"] = value;
		}

		public static void SetAppUserModelPreventPinningProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["AppUserModel/PreventPinning"] = value;
		}

		public static void SetAppUserModelRelaunchCommandProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["AppUserModel/RelaunchCommand"] = value;
		}

		public static void SetAppUserModelRelaunchDisplayNameResourceProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["AppUserModel/RelaunchDisplayNameResource"] = value;
		}

		public static void SetAppUserModelRelaunchIconResourceProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["AppUserModel/RelaunchIconResource"] = value;
		}

		public static void SetAudioChannelCountProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Audio/ChannelCount"] = value;
		}

		public static void SetAudioCompressionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Audio/Compression"] = value;
		}

		public static void SetAudioEncodingBitrateProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Audio/EncodingBitrate"] = value;
		}

		public static void SetAudioFormatProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Audio/Format"] = value;
		}

		public static void SetAudioIsVariableBitrateProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Audio/IsVariableBitrate"] = value;
		}

		public static void SetAudioPeakValueProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Audio/PeakValue"] = value;
		}

		public static void SetAudioSampleRateProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Audio/SampleRate"] = value;
		}

		public static void SetAudioSampleSizeProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Audio/SampleSize"] = value;
		}

		public static void SetAudioStreamNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Audio/StreamName"] = value;
		}

		public static void SetAudioStreamNumberProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Audio/StreamNumber"] = value;
		}

		public static void SetCalendarDurationProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Calendar/Duration"] = value;
		}

		public static void SetCalendarIsOnlineProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Calendar/IsOnline"] = value;
		}

		public static void SetCalendarIsRecurringProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Calendar/IsRecurring"] = value;
		}

		public static void SetCalendarLocationProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Calendar/Location"] = value;
		}

		public static void SetCalendarOptionalAttendeeAddressesProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Calendar/OptionalAttendeeAddresses"] = value;
		}

		public static void SetCalendarOptionalAttendeeNamesProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Calendar/OptionalAttendeeNames"] = value;
		}

		public static void SetCalendarOrganizerAddressProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Calendar/OrganizerAddress"] = value;
		}

		public static void SetCalendarOrganizerNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Calendar/OrganizerName"] = value;
		}

		public static void SetCalendarReminderTimeProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Calendar/ReminderTime"] = value;
		}

		public static void SetCalendarRequiredAttendeeAddressesProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Calendar/RequiredAttendeeAddresses"] = value;
		}

		public static void SetCalendarRequiredAttendeeNamesProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Calendar/RequiredAttendeeNames"] = value;
		}

		public static void SetCalendarResourcesProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Calendar/Resources"] = value;
		}

		public static void SetCalendarResponseStatusProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Calendar/ResponseStatus"] = value;
		}

		public static void SetCalendarShowTimeAsProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Calendar/ShowTimeAs"] = value;
		}

		public static void SetCalendarShowTimeAsTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Calendar/ShowTimeAsText"] = value;
		}

		public static void SetCommunicationAccountNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Communication/AccountName"] = value;
		}

		public static void SetCommunicationDateItemExpiresProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Communication/DateItemExpires"] = value;
		}

		public static void SetCommunicationFollowUpIconIndexProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Communication/FollowUpIconIndex"] = value;
		}

		public static void SetCommunicationHeaderItemProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Communication/HeaderItem"] = value;
		}

		public static void SetCommunicationPolicyTagProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Communication/PolicyTag"] = value;
		}

		public static void SetCommunicationSecurityFlagsProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Communication/SecurityFlags"] = value;
		}

		public static void SetCommunicationSuffixProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Communication/Suffix"] = value;
		}

		public static void SetCommunicationTaskStatusProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Communication/TaskStatus"] = value;
		}

		public static void SetCommunicationTaskStatusTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Communication/TaskStatusText"] = value;
		}

		public static void SetComputerDecoratedFreeSpaceProperty(this FileDetails fileDetails, UInt64[] value)
		{
			fileDetails["Computer/DecoratedFreeSpace"] = value;
		}

		public static void SetContactAnniversaryProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Contact/Anniversary"] = value;
		}

		public static void SetContactAssistantNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/AssistantName"] = value;
		}

		public static void SetContactAssistantTelephoneProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/AssistantTelephone"] = value;
		}

		public static void SetContactBirthdayProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Contact/Birthday"] = value;
		}

		public static void SetContactBusinessAddressProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/BusinessAddress"] = value;
		}

		public static void SetContactBusinessAddressCityProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/BusinessAddressCity"] = value;
		}

		public static void SetContactBusinessAddressCountryProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/BusinessAddressCountry"] = value;
		}

		public static void SetContactBusinessAddressPostalCodeProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/BusinessAddressPostalCode"] = value;
		}

		public static void SetContactBusinessAddressPostOfficeBoxProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/BusinessAddressPostOfficeBox"] = value;
		}

		public static void SetContactBusinessAddressStateProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/BusinessAddressState"] = value;
		}

		public static void SetContactBusinessAddressStreetProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/BusinessAddressStreet"] = value;
		}

		public static void SetContactBusinessFaxNumberProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/BusinessFaxNumber"] = value;
		}

		public static void SetContactBusinessHomepageProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/BusinessHomepage"] = value;
		}

		public static void SetContactBusinessTelephoneProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/BusinessTelephone"] = value;
		}

		public static void SetContactCallbackTelephoneProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/CallbackTelephone"] = value;
		}

		public static void SetContactCarTelephoneProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/CarTelephone"] = value;
		}

		public static void SetContactChildrenProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Contact/Children"] = value;
		}

		public static void SetContactCompanyMainTelephoneProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/CompanyMainTelephone"] = value;
		}

		public static void SetContactDepartmentProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/Department"] = value;
		}

		public static void SetContactEmailAddressProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/EmailAddress"] = value;
		}

		public static void SetContactEmailAddress2Property(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/EmailAddress2"] = value;
		}

		public static void SetContactEmailAddress3Property(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/EmailAddress3"] = value;
		}

		public static void SetContactEmailAddressesProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Contact/EmailAddresses"] = value;
		}

		public static void SetContactEmailNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/EmailName"] = value;
		}

		public static void SetContactFileAsNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/FileAsName"] = value;
		}

		public static void SetContactFirstNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/FirstName"] = value;
		}

		public static void SetContactFullNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/FullName"] = value;
		}

		public static void SetContactGenderProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/Gender"] = value;
		}

		public static void SetContactGenderValueProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Contact/GenderValue"] = value;
		}

		public static void SetContactHobbiesProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Contact/Hobbies"] = value;
		}

		public static void SetContactHomeAddressProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/HomeAddress"] = value;
		}

		public static void SetContactHomeAddressCityProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/HomeAddressCity"] = value;
		}

		public static void SetContactHomeAddressCountryProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/HomeAddressCountry"] = value;
		}

		public static void SetContactHomeAddressPostalCodeProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/HomeAddressPostalCode"] = value;
		}

		public static void SetContactHomeAddressPostOfficeBoxProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/HomeAddressPostOfficeBox"] = value;
		}

		public static void SetContactHomeAddressStateProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/HomeAddressState"] = value;
		}

		public static void SetContactHomeAddressStreetProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/HomeAddressStreet"] = value;
		}

		public static void SetContactHomeFaxNumberProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/HomeFaxNumber"] = value;
		}

		public static void SetContactHomeTelephoneProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/HomeTelephone"] = value;
		}

		public static void SetContactIMAddressProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Contact/IMAddress"] = value;
		}

		public static void SetContactInitialsProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/Initials"] = value;
		}

		public static void SetContactJobTitleProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/JobTitle"] = value;
		}

		public static void SetContactLabelProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/Label"] = value;
		}

		public static void SetContactLastNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/LastName"] = value;
		}

		public static void SetContactMailingAddressProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/MailingAddress"] = value;
		}

		public static void SetContactMiddleNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/MiddleName"] = value;
		}

		public static void SetContactMobileTelephoneProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/MobileTelephone"] = value;
		}

		public static void SetContactNicknameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/Nickname"] = value;
		}

		public static void SetContactOfficeLocationProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/OfficeLocation"] = value;
		}

		public static void SetContactOtherAddressProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/OtherAddress"] = value;
		}

		public static void SetContactOtherAddressCityProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/OtherAddressCity"] = value;
		}

		public static void SetContactOtherAddressCountryProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/OtherAddressCountry"] = value;
		}

		public static void SetContactOtherAddressPostalCodeProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/OtherAddressPostalCode"] = value;
		}

		public static void SetContactOtherAddressPostOfficeBoxProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/OtherAddressPostOfficeBox"] = value;
		}

		public static void SetContactOtherAddressStateProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/OtherAddressState"] = value;
		}

		public static void SetContactOtherAddressStreetProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/OtherAddressStreet"] = value;
		}

		public static void SetContactPagerTelephoneProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/PagerTelephone"] = value;
		}

		public static void SetContactPersonalTitleProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/PersonalTitle"] = value;
		}

		public static void SetContactPrimaryAddressCityProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/PrimaryAddressCity"] = value;
		}

		public static void SetContactPrimaryAddressCountryProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/PrimaryAddressCountry"] = value;
		}

		public static void SetContactPrimaryAddressPostalCodeProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/PrimaryAddressPostalCode"] = value;
		}

		public static void SetContactPrimaryAddressPostOfficeBoxProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/PrimaryAddressPostOfficeBox"] = value;
		}

		public static void SetContactPrimaryAddressStateProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/PrimaryAddressState"] = value;
		}

		public static void SetContactPrimaryAddressStreetProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/PrimaryAddressStreet"] = value;
		}

		public static void SetContactPrimaryEmailAddressProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/PrimaryEmailAddress"] = value;
		}

		public static void SetContactPrimaryTelephoneProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/PrimaryTelephone"] = value;
		}

		public static void SetContactProfessionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/Profession"] = value;
		}

		public static void SetContactSpouseNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/SpouseName"] = value;
		}

		public static void SetContactSuffixProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/Suffix"] = value;
		}

		public static void SetContactTelexNumberProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/TelexNumber"] = value;
		}

		public static void SetContactTTYTDDTelephoneProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/TTYTDDTelephone"] = value;
		}

		public static void SetContactWebpageProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Contact/Webpage"] = value;
		}

		public static void SetDevicePrinterUrlProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Device/PrinterUrl"] = value;
		}

		public static void SetDeviceInterfacePrinterDriverDirectoryProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["DeviceInterface/PrinterDriverDirectory"] = value;
		}

		public static void SetDeviceInterfacePrinterDriverNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["DeviceInterface/PrinterDriverName"] = value;
		}

		public static void SetDeviceInterfacePrinterNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["DeviceInterface/PrinterName"] = value;
		}

		public static void SetDeviceInterfacePrinterPortNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["DeviceInterface/PrinterPortName"] = value;
		}

		public static void SetDevicesBatteryLifeProperty(this FileDetails fileDetails, Nullable<byte> value)
		{
			fileDetails["Devices/BatteryLife"] = value;
		}

		public static void SetDevicesBatteryPlusChargingProperty(this FileDetails fileDetails, Nullable<byte> value)
		{
			fileDetails["Devices/BatteryPlusCharging"] = value;
		}

		public static void SetDevicesBatteryPlusChargingTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/BatteryPlusChargingText"] = value;
		}

		public static void SetDevicesCategoryProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Devices/Category"] = value;
		}

		public static void SetDevicesCategoryGroupProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Devices/CategoryGroup"] = value;
		}

		public static void SetDevicesCategoryPluralProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Devices/CategoryPlural"] = value;
		}

		public static void SetDevicesChargingStateProperty(this FileDetails fileDetails, Nullable<byte> value)
		{
			fileDetails["Devices/ChargingState"] = value;
		}

		public static void SetDevicesConnectedProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Devices/Connected"] = value;
		}

		public static void SetDevicesContainerIdProperty(this FileDetails fileDetails, Nullable<IntPtr> value)
		{
			fileDetails["Devices/ContainerId"] = value;
		}

		public static void SetDevicesDefaultTooltipProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/DefaultTooltip"] = value;
		}

		public static void SetDevicesDeviceDescription1Property(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/DeviceDescription1"] = value;
		}

		public static void SetDevicesDeviceDescription2Property(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/DeviceDescription2"] = value;
		}

		public static void SetDevicesDiscoveryMethodProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Devices/DiscoveryMethod"] = value;
		}

		public static void SetDevicesFriendlyNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/FriendlyName"] = value;
		}

		public static void SetDevicesFunctionPathsProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Devices/FunctionPaths"] = value;
		}

		public static void SetDevicesInterfacePathsProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Devices/InterfacePaths"] = value;
		}

		public static void SetDevicesIsDefaultProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Devices/IsDefault"] = value;
		}

		public static void SetDevicesIsNetworkConnectedProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Devices/IsNetworkConnected"] = value;
		}

		public static void SetDevicesIsSharedProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Devices/IsShared"] = value;
		}

		public static void SetDevicesIsSoftwareInstallingProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Devices/IsSoftwareInstalling"] = value;
		}

		public static void SetDevicesLaunchDeviceStageFromExplorerProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Devices/LaunchDeviceStageFromExplorer"] = value;
		}

		public static void SetDevicesLocalMachineProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Devices/LocalMachine"] = value;
		}

		public static void SetDevicesManufacturerProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/Manufacturer"] = value;
		}

		public static void SetDevicesMissedCallsProperty(this FileDetails fileDetails, Nullable<byte> value)
		{
			fileDetails["Devices/MissedCalls"] = value;
		}

		public static void SetDevicesModelNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/ModelName"] = value;
		}

		public static void SetDevicesModelNumberProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/ModelNumber"] = value;
		}

		public static void SetDevicesNetworkedTooltipProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/NetworkedTooltip"] = value;
		}

		public static void SetDevicesNetworkNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/NetworkName"] = value;
		}

		public static void SetDevicesNetworkTypeProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/NetworkType"] = value;
		}

		public static void SetDevicesNewPicturesProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Devices/NewPictures"] = value;
		}

		public static void SetDevicesNotificationProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/Notification"] = value;
		}

		public static void SetDevicesNotificationStoreProperty(this FileDetails fileDetails, Nullable<IntPtr> value)
		{
			fileDetails["Devices/NotificationStore"] = value;
		}

		public static void SetDevicesNotWorkingProperlyProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Devices/NotWorkingProperly"] = value;
		}

		public static void SetDevicesPairedProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Devices/Paired"] = value;
		}

		public static void SetDevicesPrimaryCategoryProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/PrimaryCategory"] = value;
		}

		public static void SetDevicesRoamingProperty(this FileDetails fileDetails, Nullable<byte> value)
		{
			fileDetails["Devices/Roaming"] = value;
		}

		public static void SetDevicesSafeRemovalRequiredProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Devices/SafeRemovalRequired"] = value;
		}

		public static void SetDevicesSharedTooltipProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/SharedTooltip"] = value;
		}

		public static void SetDevicesSignalStrengthProperty(this FileDetails fileDetails, Nullable<byte> value)
		{
			fileDetails["Devices/SignalStrength"] = value;
		}

		public static void SetDevicesStatus1Property(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/Status1"] = value;
		}

		public static void SetDevicesStatus2Property(this FileDetails fileDetails, string value)
		{
			fileDetails["Devices/Status2"] = value;
		}

		public static void SetDevicesStorageCapacityProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["Devices/StorageCapacity"] = value;
		}

		public static void SetDevicesStorageFreeSpaceProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["Devices/StorageFreeSpace"] = value;
		}

		public static void SetDevicesStorageFreeSpacePercentProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Devices/StorageFreeSpacePercent"] = value;
		}

		public static void SetDevicesTextMessagesProperty(this FileDetails fileDetails, Nullable<byte> value)
		{
			fileDetails["Devices/TextMessages"] = value;
		}

		public static void SetDevicesVoicemailProperty(this FileDetails fileDetails, Nullable<byte> value)
		{
			fileDetails["Devices/Voicemail"] = value;
		}

		public static void SetDocumentByteCountProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Document/ByteCount"] = value;
		}

		public static void SetDocumentCharacterCountProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Document/CharacterCount"] = value;
		}

		public static void SetDocumentClientIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Document/ClientID"] = value;
		}

		public static void SetDocumentContributorProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Document/Contributor"] = value;
		}

		public static void SetDocumentDateCreatedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Document/DateCreated"] = value;
		}

		public static void SetDocumentDatePrintedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Document/DatePrinted"] = value;
		}

		public static void SetDocumentDateSavedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Document/DateSaved"] = value;
		}

		public static void SetDocumentDivisionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Document/Division"] = value;
		}

		public static void SetDocumentDocumentIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Document/DocumentID"] = value;
		}

		public static void SetDocumentHiddenSlideCountProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Document/HiddenSlideCount"] = value;
		}

		public static void SetDocumentLastAuthorProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Document/LastAuthor"] = value;
		}

		public static void SetDocumentLineCountProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Document/LineCount"] = value;
		}

		public static void SetDocumentManagerProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Document/Manager"] = value;
		}

		public static void SetDocumentMultimediaClipCountProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Document/MultimediaClipCount"] = value;
		}

		public static void SetDocumentNoteCountProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Document/NoteCount"] = value;
		}

		public static void SetDocumentPageCountProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Document/PageCount"] = value;
		}

		public static void SetDocumentParagraphCountProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Document/ParagraphCount"] = value;
		}

		public static void SetDocumentPresentationFormatProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Document/PresentationFormat"] = value;
		}

		public static void SetDocumentRevisionNumberProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Document/RevisionNumber"] = value;
		}

		public static void SetDocumentSecurityProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Document/Security"] = value;
		}

		public static void SetDocumentSlideCountProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Document/SlideCount"] = value;
		}

		public static void SetDocumentTemplateProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Document/Template"] = value;
		}

		public static void SetDocumentTotalEditingTimeProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["Document/TotalEditingTime"] = value;
		}

		public static void SetDocumentVersionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Document/Version"] = value;
		}

		public static void SetDocumentWordCountProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Document/WordCount"] = value;
		}

		public static void SetDRMDatePlayExpiresProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["DRM/DatePlayExpires"] = value;
		}

		public static void SetDRMDatePlayStartsProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["DRM/DatePlayStarts"] = value;
		}

		public static void SetDRMDescriptionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["DRM/Description"] = value;
		}

		public static void SetDRMIsProtectedProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["DRM/IsProtected"] = value;
		}

		public static void SetDRMPlayCountProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["DRM/PlayCount"] = value;
		}

		public static void SetGPSAltitudeProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["GPS/Altitude"] = value;
		}

		public static void SetGPSAltitudeDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/AltitudeDenominator"] = value;
		}

		public static void SetGPSAltitudeNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/AltitudeNumerator"] = value;
		}

		public static void SetGPSAltitudeRefProperty(this FileDetails fileDetails, Nullable<byte> value)
		{
			fileDetails["GPS/AltitudeRef"] = value;
		}

		public static void SetGPSAreaInformationProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/AreaInformation"] = value;
		}

		public static void SetGPSDateProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["GPS/Date"] = value;
		}

		public static void SetGPSDestinationBearingProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["GPS/DestinationBearing"] = value;
		}

		public static void SetGPSDestinationBearingDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/DestinationBearingDenominator"] = value;
		}

		public static void SetGPSDestinationBearingNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/DestinationBearingNumerator"] = value;
		}

		public static void SetGPSDestinationBearingRefProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/DestinationBearingRef"] = value;
		}

		public static void SetGPSDestinationDistanceProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["GPS/DestinationDistance"] = value;
		}

		public static void SetGPSDestinationDistanceDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/DestinationDistanceDenominator"] = value;
		}

		public static void SetGPSDestinationDistanceNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/DestinationDistanceNumerator"] = value;
		}

		public static void SetGPSDestinationDistanceRefProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/DestinationDistanceRef"] = value;
		}

		public static void SetGPSDestinationLatitudeProperty(this FileDetails fileDetails, Double[] value)
		{
			fileDetails["GPS/DestinationLatitude"] = value;
		}

		public static void SetGPSDestinationLatitudeDenominatorProperty(this FileDetails fileDetails, UInt32[] value)
		{
			fileDetails["GPS/DestinationLatitudeDenominator"] = value;
		}

		public static void SetGPSDestinationLatitudeNumeratorProperty(this FileDetails fileDetails, UInt32[] value)
		{
			fileDetails["GPS/DestinationLatitudeNumerator"] = value;
		}

		public static void SetGPSDestinationLatitudeRefProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/DestinationLatitudeRef"] = value;
		}

		public static void SetGPSDestinationLongitudeProperty(this FileDetails fileDetails, Double[] value)
		{
			fileDetails["GPS/DestinationLongitude"] = value;
		}

		public static void SetGPSDestinationLongitudeDenominatorProperty(this FileDetails fileDetails, UInt32[] value)
		{
			fileDetails["GPS/DestinationLongitudeDenominator"] = value;
		}

		public static void SetGPSDestinationLongitudeNumeratorProperty(this FileDetails fileDetails, UInt32[] value)
		{
			fileDetails["GPS/DestinationLongitudeNumerator"] = value;
		}

		public static void SetGPSDestinationLongitudeRefProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/DestinationLongitudeRef"] = value;
		}

		public static void SetGPSDifferentialProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["GPS/Differential"] = value;
		}

		public static void SetGPSDOPProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["GPS/DOP"] = value;
		}

		public static void SetGPSDOPDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/DOPDenominator"] = value;
		}

		public static void SetGPSDOPNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/DOPNumerator"] = value;
		}

		public static void SetGPSImageDirectionProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["GPS/ImageDirection"] = value;
		}

		public static void SetGPSImageDirectionDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/ImageDirectionDenominator"] = value;
		}

		public static void SetGPSImageDirectionNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/ImageDirectionNumerator"] = value;
		}

		public static void SetGPSImageDirectionRefProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/ImageDirectionRef"] = value;
		}

		public static void SetGPSLatitudeProperty(this FileDetails fileDetails, Double[] value)
		{
			fileDetails["GPS/Latitude"] = value;
		}

		public static void SetGPSLatitudeDenominatorProperty(this FileDetails fileDetails, UInt32[] value)
		{
			fileDetails["GPS/LatitudeDenominator"] = value;
		}

		public static void SetGPSLatitudeNumeratorProperty(this FileDetails fileDetails, UInt32[] value)
		{
			fileDetails["GPS/LatitudeNumerator"] = value;
		}

		public static void SetGPSLatitudeRefProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/LatitudeRef"] = value;
		}

		public static void SetGPSLongitudeProperty(this FileDetails fileDetails, Double[] value)
		{
			fileDetails["GPS/Longitude"] = value;
		}

		public static void SetGPSLongitudeDenominatorProperty(this FileDetails fileDetails, UInt32[] value)
		{
			fileDetails["GPS/LongitudeDenominator"] = value;
		}

		public static void SetGPSLongitudeNumeratorProperty(this FileDetails fileDetails, UInt32[] value)
		{
			fileDetails["GPS/LongitudeNumerator"] = value;
		}

		public static void SetGPSLongitudeRefProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/LongitudeRef"] = value;
		}

		public static void SetGPSMapDatumProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/MapDatum"] = value;
		}

		public static void SetGPSMeasureModeProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/MeasureMode"] = value;
		}

		public static void SetGPSProcessingMethodProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/ProcessingMethod"] = value;
		}

		public static void SetGPSSatellitesProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/Satellites"] = value;
		}

		public static void SetGPSSpeedProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["GPS/Speed"] = value;
		}

		public static void SetGPSSpeedDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/SpeedDenominator"] = value;
		}

		public static void SetGPSSpeedNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/SpeedNumerator"] = value;
		}

		public static void SetGPSSpeedRefProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/SpeedRef"] = value;
		}

		public static void SetGPSStatusProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/Status"] = value;
		}

		public static void SetGPSTrackProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["GPS/Track"] = value;
		}

		public static void SetGPSTrackDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/TrackDenominator"] = value;
		}

		public static void SetGPSTrackNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["GPS/TrackNumerator"] = value;
		}

		public static void SetGPSTrackRefProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["GPS/TrackRef"] = value;
		}

		public static void SetGPSVersionIDProperty(this FileDetails fileDetails, Byte[] value)
		{
			fileDetails["GPS/VersionID"] = value;
		}

		public static void SetIdentityBlobProperty(this FileDetails fileDetails, Byte[] value)
		{
			fileDetails["Identity/Blob"] = value;
		}

		public static void SetIdentityDisplayNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Identity/DisplayName"] = value;
		}

		public static void SetIdentityIsMeIdentityProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Identity/IsMeIdentity"] = value;
		}

		public static void SetIdentityPrimaryEmailAddressProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Identity/PrimaryEmailAddress"] = value;
		}

		public static void SetIdentityProviderIDProperty(this FileDetails fileDetails, Nullable<IntPtr> value)
		{
			fileDetails["Identity/ProviderID"] = value;
		}

		public static void SetIdentityUniqueIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Identity/UniqueID"] = value;
		}

		public static void SetIdentityUserNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Identity/UserName"] = value;
		}

		public static void SetIdentityProviderNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["IdentityProvider/Name"] = value;
		}

		public static void SetIdentityProviderPictureProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["IdentityProvider/Picture"] = value;
		}

		public static void SetImageBitDepthProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Image/BitDepth"] = value;
		}

		public static void SetImageColorSpaceProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Image/ColorSpace"] = value;
		}

		public static void SetImageCompressedBitsPerPixelProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Image/CompressedBitsPerPixel"] = value;
		}

		public static void SetImageCompressedBitsPerPixelDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Image/CompressedBitsPerPixelDenominator"] = value;
		}

		public static void SetImageCompressedBitsPerPixelNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Image/CompressedBitsPerPixelNumerator"] = value;
		}

		public static void SetImageCompressionProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Image/Compression"] = value;
		}

		public static void SetImageCompressionTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Image/CompressionText"] = value;
		}

		public static void SetImageDimensionsProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Image/Dimensions"] = value;
		}

		public static void SetImageHorizontalResolutionProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Image/HorizontalResolution"] = value;
		}

		public static void SetImageHorizontalSizeProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Image/HorizontalSize"] = value;
		}

		public static void SetImageImageIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Image/ImageID"] = value;
		}

		public static void SetImageResolutionUnitProperty(this FileDetails fileDetails, Nullable<short> value)
		{
			fileDetails["Image/ResolutionUnit"] = value;
		}

		public static void SetImageVerticalResolutionProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Image/VerticalResolution"] = value;
		}

		public static void SetImageVerticalSizeProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Image/VerticalSize"] = value;
		}

		public static void SetJournalContactsProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Journal/Contacts"] = value;
		}

		public static void SetJournalEntryTypeProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Journal/EntryType"] = value;
		}

		public static void SetLayoutPatternContentViewModeForBrowseProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["LayoutPattern/ContentViewModeForBrowse"] = value;
		}

		public static void SetLayoutPatternContentViewModeForSearchProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["LayoutPattern/ContentViewModeForSearch"] = value;
		}

		public static void SetLinkArgumentsProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Link/Arguments"] = value;
		}

		public static void SetLinkCommentProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Link/Comment"] = value;
		}

		public static void SetLinkDateVisitedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Link/DateVisited"] = value;
		}

		public static void SetLinkDescriptionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Link/Description"] = value;
		}

		public static void SetLinkStatusProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Link/Status"] = value;
		}

		public static void SetLinkTargetExtensionProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Link/TargetExtension"] = value;
		}

		public static void SetLinkTargetParsingPathProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Link/TargetParsingPath"] = value;
		}

		public static void SetLinkTargetSFGAOFlagsProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Link/TargetSFGAOFlags"] = value;
		}

		public static void SetLinkTargetSFGAOFlagsStringsProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Link/TargetSFGAOFlagsStrings"] = value;
		}

		public static void SetLinkTargetUrlProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Link/TargetUrl"] = value;
		}

		public static void SetMediaAuthorUrlProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/AuthorUrl"] = value;
		}

		public static void SetMediaAverageLevelProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Media/AverageLevel"] = value;
		}

		public static void SetMediaClassPrimaryIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/ClassPrimaryID"] = value;
		}

		public static void SetMediaClassSecondaryIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/ClassSecondaryID"] = value;
		}

		public static void SetMediaCollectionGroupIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/CollectionGroupID"] = value;
		}

		public static void SetMediaCollectionIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/CollectionID"] = value;
		}

		public static void SetMediaContentDistributorProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/ContentDistributor"] = value;
		}

		public static void SetMediaContentIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/ContentID"] = value;
		}

		public static void SetMediaCreatorApplicationProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/CreatorApplication"] = value;
		}

		public static void SetMediaCreatorApplicationVersionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/CreatorApplicationVersion"] = value;
		}

		public static void SetMediaDateEncodedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Media/DateEncoded"] = value;
		}

		public static void SetMediaDateReleasedProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/DateReleased"] = value;
		}

		public static void SetMediaDurationProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["Media/Duration"] = value;
		}

		public static void SetMediaDVDIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/DVDID"] = value;
		}

		public static void SetMediaEncodedByProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/EncodedBy"] = value;
		}

		public static void SetMediaEncodingSettingsProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/EncodingSettings"] = value;
		}

		public static void SetMediaFrameCountProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Media/FrameCount"] = value;
		}

		public static void SetMediaMCDIProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/MCDI"] = value;
		}

		public static void SetMediaMetadataContentProviderProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/MetadataContentProvider"] = value;
		}

		public static void SetMediaProducerProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Media/Producer"] = value;
		}

		public static void SetMediaPromotionUrlProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/PromotionUrl"] = value;
		}

		public static void SetMediaProtectionTypeProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/ProtectionType"] = value;
		}

		public static void SetMediaProviderRatingProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/ProviderRating"] = value;
		}

		public static void SetMediaProviderStyleProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/ProviderStyle"] = value;
		}

		public static void SetMediaPublisherProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/Publisher"] = value;
		}

		public static void SetMediaSubscriptionContentIdProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/SubscriptionContentId"] = value;
		}

		public static void SetMediaSubtitleProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/Subtitle"] = value;
		}

		public static void SetMediaUniqueFileIdentifierProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/UniqueFileIdentifier"] = value;
		}

		public static void SetMediaUserNoAutoInfoProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/UserNoAutoInfo"] = value;
		}

		public static void SetMediaUserWebUrlProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Media/UserWebUrl"] = value;
		}

		public static void SetMediaWriterProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Media/Writer"] = value;
		}

		public static void SetMediaYearProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Media/Year"] = value;
		}

		public static void SetMessageAttachmentContentsProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Message/AttachmentContents"] = value;
		}

		public static void SetMessageAttachmentNamesProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Message/AttachmentNames"] = value;
		}

		public static void SetMessageBccAddressProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Message/BccAddress"] = value;
		}

		public static void SetMessageBccNameProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Message/BccName"] = value;
		}

		public static void SetMessageCcAddressProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Message/CcAddress"] = value;
		}

		public static void SetMessageCcNameProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Message/CcName"] = value;
		}

		public static void SetMessageConversationIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Message/ConversationID"] = value;
		}

		public static void SetMessageConversationIndexProperty(this FileDetails fileDetails, Byte[] value)
		{
			fileDetails["Message/ConversationIndex"] = value;
		}

		public static void SetMessageDateReceivedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Message/DateReceived"] = value;
		}

		public static void SetMessageDateSentProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Message/DateSent"] = value;
		}

		public static void SetMessageFlagsProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Message/Flags"] = value;
		}

		public static void SetMessageFromAddressProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Message/FromAddress"] = value;
		}

		public static void SetMessageFromNameProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Message/FromName"] = value;
		}

		public static void SetMessageHasAttachmentsProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Message/HasAttachments"] = value;
		}

		public static void SetMessageIsFwdOrReplyProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Message/IsFwdOrReply"] = value;
		}

		public static void SetMessageMessageClassProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Message/MessageClass"] = value;
		}

		public static void SetMessageProofInProgressProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Message/ProofInProgress"] = value;
		}

		public static void SetMessageSenderAddressProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Message/SenderAddress"] = value;
		}

		public static void SetMessageSenderNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Message/SenderName"] = value;
		}

		public static void SetMessageStoreProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Message/Store"] = value;
		}

		public static void SetMessageToAddressProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Message/ToAddress"] = value;
		}

		public static void SetMessageToDoFlagsProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Message/ToDoFlags"] = value;
		}

		public static void SetMessageToDoTitleProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Message/ToDoTitle"] = value;
		}

		public static void SetMessageToNameProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Message/ToName"] = value;
		}

		public static void SetMusicAlbumArtistProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Music/AlbumArtist"] = value;
		}

		public static void SetMusicAlbumIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Music/AlbumID"] = value;
		}

		public static void SetMusicAlbumTitleProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Music/AlbumTitle"] = value;
		}

		public static void SetMusicArtistProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Music/Artist"] = value;
		}

		public static void SetMusicBeatsPerMinuteProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Music/BeatsPerMinute"] = value;
		}

		public static void SetMusicComposerProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Music/Composer"] = value;
		}

		public static void SetMusicConductorProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Music/Conductor"] = value;
		}

		public static void SetMusicContentGroupDescriptionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Music/ContentGroupDescription"] = value;
		}

		public static void SetMusicDisplayArtistProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Music/DisplayArtist"] = value;
		}

		public static void SetMusicGenreProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Music/Genre"] = value;
		}

		public static void SetMusicInitialKeyProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Music/InitialKey"] = value;
		}

		public static void SetMusicIsCompilationProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Music/IsCompilation"] = value;
		}

		public static void SetMusicLyricsProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Music/Lyrics"] = value;
		}

		public static void SetMusicMoodProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Music/Mood"] = value;
		}

		public static void SetMusicPartOfSetProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Music/PartOfSet"] = value;
		}

		public static void SetMusicPeriodProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Music/Period"] = value;
		}

		public static void SetMusicSynchronizedLyricsProperty(this FileDetails fileDetails, Byte[] value)
		{
			fileDetails["Music/SynchronizedLyrics"] = value;
		}

		public static void SetMusicTrackNumberProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Music/TrackNumber"] = value;
		}

		public static void SetNoteColorProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Note/Color"] = value;
		}

		public static void SetNoteColorTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Note/ColorText"] = value;
		}

		public static void SetPhotoApertureProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/Aperture"] = value;
		}

		public static void SetPhotoApertureDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/ApertureDenominator"] = value;
		}

		public static void SetPhotoApertureNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/ApertureNumerator"] = value;
		}

		public static void SetPhotoBrightnessProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/Brightness"] = value;
		}

		public static void SetPhotoBrightnessDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/BrightnessDenominator"] = value;
		}

		public static void SetPhotoBrightnessNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/BrightnessNumerator"] = value;
		}

		public static void SetPhotoCameraManufacturerProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/CameraManufacturer"] = value;
		}

		public static void SetPhotoCameraModelProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/CameraModel"] = value;
		}

		public static void SetPhotoCameraSerialNumberProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/CameraSerialNumber"] = value;
		}

		public static void SetPhotoContrastProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/Contrast"] = value;
		}

		public static void SetPhotoContrastTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/ContrastText"] = value;
		}

		public static void SetPhotoDateTakenProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Photo/DateTaken"] = value;
		}

		public static void SetPhotoDigitalZoomProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/DigitalZoom"] = value;
		}

		public static void SetPhotoDigitalZoomDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/DigitalZoomDenominator"] = value;
		}

		public static void SetPhotoDigitalZoomNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/DigitalZoomNumerator"] = value;
		}

		public static void SetPhotoEventProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Photo/Event"] = value;
		}

		public static void SetPhotoEXIFVersionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/EXIFVersion"] = value;
		}

		public static void SetPhotoExposureBiasProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/ExposureBias"] = value;
		}

		public static void SetPhotoExposureBiasDenominatorProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Photo/ExposureBiasDenominator"] = value;
		}

		public static void SetPhotoExposureBiasNumeratorProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Photo/ExposureBiasNumerator"] = value;
		}

		public static void SetPhotoExposureIndexProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/ExposureIndex"] = value;
		}

		public static void SetPhotoExposureIndexDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/ExposureIndexDenominator"] = value;
		}

		public static void SetPhotoExposureIndexNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/ExposureIndexNumerator"] = value;
		}

		public static void SetPhotoExposureProgramProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/ExposureProgram"] = value;
		}

		public static void SetPhotoExposureProgramTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/ExposureProgramText"] = value;
		}

		public static void SetPhotoExposureTimeProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/ExposureTime"] = value;
		}

		public static void SetPhotoExposureTimeDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/ExposureTimeDenominator"] = value;
		}

		public static void SetPhotoExposureTimeNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/ExposureTimeNumerator"] = value;
		}

		public static void SetPhotoFlashProperty(this FileDetails fileDetails, Nullable<byte> value)
		{
			fileDetails["Photo/Flash"] = value;
		}

		public static void SetPhotoFlashEnergyProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/FlashEnergy"] = value;
		}

		public static void SetPhotoFlashEnergyDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/FlashEnergyDenominator"] = value;
		}

		public static void SetPhotoFlashEnergyNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/FlashEnergyNumerator"] = value;
		}

		public static void SetPhotoFlashManufacturerProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/FlashManufacturer"] = value;
		}

		public static void SetPhotoFlashModelProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/FlashModel"] = value;
		}

		public static void SetPhotoFlashTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/FlashText"] = value;
		}

		public static void SetPhotoFNumberProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/FNumber"] = value;
		}

		public static void SetPhotoFNumberDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/FNumberDenominator"] = value;
		}

		public static void SetPhotoFNumberNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/FNumberNumerator"] = value;
		}

		public static void SetPhotoFocalLengthProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/FocalLength"] = value;
		}

		public static void SetPhotoFocalLengthDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/FocalLengthDenominator"] = value;
		}

		public static void SetPhotoFocalLengthInFilmProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Photo/FocalLengthInFilm"] = value;
		}

		public static void SetPhotoFocalLengthNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/FocalLengthNumerator"] = value;
		}

		public static void SetPhotoFocalPlaneXResolutionProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/FocalPlaneXResolution"] = value;
		}

		public static void SetPhotoFocalPlaneXResolutionDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/FocalPlaneXResolutionDenominator"] = value;
		}

		public static void SetPhotoFocalPlaneXResolutionNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/FocalPlaneXResolutionNumerator"] = value;
		}

		public static void SetPhotoFocalPlaneYResolutionProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/FocalPlaneYResolution"] = value;
		}

		public static void SetPhotoFocalPlaneYResolutionDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/FocalPlaneYResolutionDenominator"] = value;
		}

		public static void SetPhotoFocalPlaneYResolutionNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/FocalPlaneYResolutionNumerator"] = value;
		}

		public static void SetPhotoGainControlProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/GainControl"] = value;
		}

		public static void SetPhotoGainControlDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/GainControlDenominator"] = value;
		}

		public static void SetPhotoGainControlNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/GainControlNumerator"] = value;
		}

		public static void SetPhotoGainControlTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/GainControlText"] = value;
		}

		public static void SetPhotoISOSpeedProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Photo/ISOSpeed"] = value;
		}

		public static void SetPhotoLensManufacturerProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/LensManufacturer"] = value;
		}

		public static void SetPhotoLensModelProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/LensModel"] = value;
		}

		public static void SetPhotoLightSourceProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/LightSource"] = value;
		}

		public static void SetPhotoMakerNoteProperty(this FileDetails fileDetails, Byte[] value)
		{
			fileDetails["Photo/MakerNote"] = value;
		}

		public static void SetPhotoMakerNoteOffsetProperty(this FileDetails fileDetails, Nullable<ulong> value)
		{
			fileDetails["Photo/MakerNoteOffset"] = value;
		}

		public static void SetPhotoMaxApertureProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/MaxAperture"] = value;
		}

		public static void SetPhotoMaxApertureDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/MaxApertureDenominator"] = value;
		}

		public static void SetPhotoMaxApertureNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/MaxApertureNumerator"] = value;
		}

		public static void SetPhotoMeteringModeProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Photo/MeteringMode"] = value;
		}

		public static void SetPhotoMeteringModeTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/MeteringModeText"] = value;
		}

		public static void SetPhotoOrientationProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Photo/Orientation"] = value;
		}

		public static void SetPhotoOrientationTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/OrientationText"] = value;
		}

		public static void SetPhotoPeopleNamesProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Photo/PeopleNames"] = value;
		}

		public static void SetPhotoPhotometricInterpretationProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Photo/PhotometricInterpretation"] = value;
		}

		public static void SetPhotoPhotometricInterpretationTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/PhotometricInterpretationText"] = value;
		}

		public static void SetPhotoProgramModeProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/ProgramMode"] = value;
		}

		public static void SetPhotoProgramModeTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/ProgramModeText"] = value;
		}

		public static void SetPhotoRelatedSoundFileProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/RelatedSoundFile"] = value;
		}

		public static void SetPhotoSaturationProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/Saturation"] = value;
		}

		public static void SetPhotoSaturationTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/SaturationText"] = value;
		}

		public static void SetPhotoSharpnessProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/Sharpness"] = value;
		}

		public static void SetPhotoSharpnessTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/SharpnessText"] = value;
		}

		public static void SetPhotoShutterSpeedProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/ShutterSpeed"] = value;
		}

		public static void SetPhotoShutterSpeedDenominatorProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Photo/ShutterSpeedDenominator"] = value;
		}

		public static void SetPhotoShutterSpeedNumeratorProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Photo/ShutterSpeedNumerator"] = value;
		}

		public static void SetPhotoSubjectDistanceProperty(this FileDetails fileDetails, Nullable<double> value)
		{
			fileDetails["Photo/SubjectDistance"] = value;
		}

		public static void SetPhotoSubjectDistanceDenominatorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/SubjectDistanceDenominator"] = value;
		}

		public static void SetPhotoSubjectDistanceNumeratorProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/SubjectDistanceNumerator"] = value;
		}

		public static void SetPhotoTagViewAggregateProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Photo/TagViewAggregate"] = value;
		}

		public static void SetPhotoTranscodedForSyncProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Photo/TranscodedForSync"] = value;
		}

		public static void SetPhotoWhiteBalanceProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Photo/WhiteBalance"] = value;
		}

		public static void SetPhotoWhiteBalanceTextProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Photo/WhiteBalanceText"] = value;
		}

		public static void SetPropGroupAdvancedProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Advanced"] = value;
		}

		public static void SetPropGroupAudioProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Audio"] = value;
		}

		public static void SetPropGroupCalendarProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Calendar"] = value;
		}

		public static void SetPropGroupCameraProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Camera"] = value;
		}

		public static void SetPropGroupContactProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Contact"] = value;
		}

		public static void SetPropGroupContentProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Content"] = value;
		}

		public static void SetPropGroupDescriptionProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Description"] = value;
		}

		public static void SetPropGroupFileSystemProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/FileSystem"] = value;
		}

		public static void SetPropGroupGeneralProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/General"] = value;
		}

		public static void SetPropGroupGPSProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/GPS"] = value;
		}

		public static void SetPropGroupImageProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Image"] = value;
		}

		public static void SetPropGroupMediaProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Media"] = value;
		}

		public static void SetPropGroupMediaAdvancedProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/MediaAdvanced"] = value;
		}

		public static void SetPropGroupMessageProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Message"] = value;
		}

		public static void SetPropGroupMusicProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Music"] = value;
		}

		public static void SetPropGroupOriginProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Origin"] = value;
		}

		public static void SetPropGroupPhotoAdvancedProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/PhotoAdvanced"] = value;
		}

		public static void SetPropGroupRecordedTVProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/RecordedTV"] = value;
		}

		public static void SetPropGroupVideoProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["PropGroup/Video"] = value;
		}

		public static void SetPropListConflictPromptProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/ConflictPrompt"] = value;
		}

		public static void SetPropListContentViewModeForBrowseProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/ContentViewModeForBrowse"] = value;
		}

		public static void SetPropListContentViewModeForSearchProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/ContentViewModeForSearch"] = value;
		}

		public static void SetPropListExtendedTileInfoProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/ExtendedTileInfo"] = value;
		}

		public static void SetPropListFileOperationPromptProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/FileOperationPrompt"] = value;
		}

		public static void SetPropListFullDetailsProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/FullDetails"] = value;
		}

		public static void SetPropListInfoTipProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/InfoTip"] = value;
		}

		public static void SetPropListNonPersonalProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/NonPersonal"] = value;
		}

		public static void SetPropListPreviewDetailsProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/PreviewDetails"] = value;
		}

		public static void SetPropListPreviewTitleProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/PreviewTitle"] = value;
		}

		public static void SetPropListQuickTipProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/QuickTip"] = value;
		}

		public static void SetPropListTileInfoProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/TileInfo"] = value;
		}

		public static void SetPropListXPDetailsPanelProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["PropList/XPDetailsPanel"] = value;
		}

		public static void SetRecordedTVChannelNumberProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["RecordedTV/ChannelNumber"] = value;
		}

		public static void SetRecordedTVCreditsProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["RecordedTV/Credits"] = value;
		}

		public static void SetRecordedTVDateContentExpiresProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["RecordedTV/DateContentExpires"] = value;
		}

		public static void SetRecordedTVEpisodeNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["RecordedTV/EpisodeName"] = value;
		}

		public static void SetRecordedTVIsATSCContentProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["RecordedTV/IsATSCContent"] = value;
		}

		public static void SetRecordedTVIsClosedCaptioningAvailableProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["RecordedTV/IsClosedCaptioningAvailable"] = value;
		}

		public static void SetRecordedTVIsDTVContentProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["RecordedTV/IsDTVContent"] = value;
		}

		public static void SetRecordedTVIsHDContentProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["RecordedTV/IsHDContent"] = value;
		}

		public static void SetRecordedTVIsRepeatBroadcastProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["RecordedTV/IsRepeatBroadcast"] = value;
		}

		public static void SetRecordedTVIsSAPProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["RecordedTV/IsSAP"] = value;
		}

		public static void SetRecordedTVNetworkAffiliationProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["RecordedTV/NetworkAffiliation"] = value;
		}

		public static void SetRecordedTVOriginalBroadcastDateProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["RecordedTV/OriginalBroadcastDate"] = value;
		}

		public static void SetRecordedTVProgramDescriptionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["RecordedTV/ProgramDescription"] = value;
		}

		public static void SetRecordedTVRecordingTimeProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["RecordedTV/RecordingTime"] = value;
		}

		public static void SetRecordedTVStationCallSignProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["RecordedTV/StationCallSign"] = value;
		}

		public static void SetRecordedTVStationNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["RecordedTV/StationName"] = value;
		}

		public static void SetSearchAutoSummaryProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Search/AutoSummary"] = value;
		}

		public static void SetSearchContainerHashProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Search/ContainerHash"] = value;
		}

		public static void SetSearchContentsProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Search/Contents"] = value;
		}

		public static void SetSearchEntryIDProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Search/EntryID"] = value;
		}

		public static void SetSearchExtendedPropertiesProperty(this FileDetails fileDetails, Byte[] value)
		{
			fileDetails["Search/ExtendedProperties"] = value;
		}

		public static void SetSearchGatherTimeProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Search/GatherTime"] = value;
		}

		public static void SetSearchHitCountProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Search/HitCount"] = value;
		}

		public static void SetSearchIsClosedDirectoryProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Search/IsClosedDirectory"] = value;
		}

		public static void SetSearchIsFullyContainedProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Search/IsFullyContained"] = value;
		}

		public static void SetSearchQueryFocusedSummaryProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Search/QueryFocusedSummary"] = value;
		}

		public static void SetSearchQueryFocusedSummaryWithFallbackProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Search/QueryFocusedSummaryWithFallback"] = value;
		}

		public static void SetSearchRankProperty(this FileDetails fileDetails, Nullable<int> value)
		{
			fileDetails["Search/Rank"] = value;
		}

		public static void SetSearchStoreProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Search/Store"] = value;
		}

		public static void SetSearchUrlToIndexProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Search/UrlToIndex"] = value;
		}

		public static void SetSearchUrlToIndexWithModificationTimeProperty(this FileDetails fileDetails, object value)
		{
			fileDetails["Search/UrlToIndexWithModificationTime"] = value;
		}

		public static void SetShellOmitFromViewProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Shell/OmitFromView"] = value;
		}

		public static void SetShellSFGAOFlagsStringsProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Shell/SFGAOFlagsStrings"] = value;
		}

		public static void SetSoftwareDateLastUsedProperty(this FileDetails fileDetails, Nullable<DateTime> value)
		{
			fileDetails["Software/DateLastUsed"] = value;
		}

		public static void SetSoftwareProductNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Software/ProductName"] = value;
		}

		public static void SetSyncCommentsProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Sync/Comments"] = value;
		}

		public static void SetSyncConflictDescriptionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Sync/ConflictDescription"] = value;
		}

		public static void SetSyncConflictFirstLocationProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Sync/ConflictFirstLocation"] = value;
		}

		public static void SetSyncConflictSecondLocationProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Sync/ConflictSecondLocation"] = value;
		}

		public static void SetSyncHandlerCollectionIDProperty(this FileDetails fileDetails, Nullable<IntPtr> value)
		{
			fileDetails["Sync/HandlerCollectionID"] = value;
		}

		public static void SetSyncHandlerIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Sync/HandlerID"] = value;
		}

		public static void SetSyncHandlerNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Sync/HandlerName"] = value;
		}

		public static void SetSyncHandlerTypeProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Sync/HandlerType"] = value;
		}

		public static void SetSyncHandlerTypeLabelProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Sync/HandlerTypeLabel"] = value;
		}

		public static void SetSyncItemIDProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Sync/ItemID"] = value;
		}

		public static void SetSyncItemNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Sync/ItemName"] = value;
		}

		public static void SetSyncProgressPercentageProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Sync/ProgressPercentage"] = value;
		}

		public static void SetSyncStateProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Sync/State"] = value;
		}

		public static void SetSyncStatusProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Sync/Status"] = value;
		}

		public static void SetTaskBillingInformationProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Task/BillingInformation"] = value;
		}

		public static void SetTaskCompletionStatusProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Task/CompletionStatus"] = value;
		}

		public static void SetTaskOwnerProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Task/Owner"] = value;
		}

		public static void SetVideoCompressionProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Video/Compression"] = value;
		}

		public static void SetVideoDirectorProperty(this FileDetails fileDetails, String[] value)
		{
			fileDetails["Video/Director"] = value;
		}

		public static void SetVideoEncodingBitrateProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Video/EncodingBitrate"] = value;
		}

		public static void SetVideoFourCCProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Video/FourCC"] = value;
		}

		public static void SetVideoFrameHeightProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Video/FrameHeight"] = value;
		}

		public static void SetVideoFrameRateProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Video/FrameRate"] = value;
		}

		public static void SetVideoFrameWidthProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Video/FrameWidth"] = value;
		}

		public static void SetVideoHorizontalAspectRatioProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Video/HorizontalAspectRatio"] = value;
		}

		public static void SetVideoSampleSizeProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Video/SampleSize"] = value;
		}

		public static void SetVideoStreamNameProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Video/StreamName"] = value;
		}

		public static void SetVideoStreamNumberProperty(this FileDetails fileDetails, Nullable<ushort> value)
		{
			fileDetails["Video/StreamNumber"] = value;
		}

		public static void SetVideoTotalBitrateProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Video/TotalBitrate"] = value;
		}

		public static void SetVideoTranscodedForSyncProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Video/TranscodedForSync"] = value;
		}

		public static void SetVideoVerticalAspectRatioProperty(this FileDetails fileDetails, Nullable<uint> value)
		{
			fileDetails["Video/VerticalAspectRatio"] = value;
		}

		public static void SetVolumeFileSystemProperty(this FileDetails fileDetails, string value)
		{
			fileDetails["Volume/FileSystem"] = value;
		}

		public static void SetVolumeIsMappedDriveProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Volume/IsMappedDrive"] = value;
		}

		public static void SetVolumeIsRootProperty(this FileDetails fileDetails, Nullable<bool> value)
		{
			fileDetails["Volume/IsRoot"] = value;
		}
	}
}
