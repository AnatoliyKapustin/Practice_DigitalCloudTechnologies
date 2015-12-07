﻿namespace DatMailReader.DataAccess.Core
{
    public class Mapi
    {
        public enum ID
        {
            PR_ACKNOWLEDGEMENT_MODE = 1,
            PR_LID_OWNER_CRITICAL_CHANGE = 1,
            PR_ALTERNATE_RECIPIENT_ALLOWED = 2,
            PR_AUTHORIZING_USERS = 3,
            PR_AUTO_FORWARD_COMMENT = 4,
            PR_AUTO_FORWARD_COMMENT_A = 4,
            PR_AUTO_FORWARD_COMMENT_W = 4,
            PR_AUTO_FORWARDED = 5,
            PR_CONTENT_CONFIDENTIALITY_ALGORITHM_ID = 6,
            PR_CONTENT_CORRELATOR = 7,
            PR_CONTENT_IDENTIFIER = 8,
            PR_CONTENT_IDENTIFIER_A = 8,
            PR_CONTENT_IDENTIFIER_W = 8,
            PR_CONTENT_LENGTH = 9,
            PR_CONTENT_RETURN_REQUESTED = 10,
            PR_CONVERSATION_KEY = 11,
            PR_CONVERSION_EITS = 12,
            PR_CONVERSION_WITH_LOSS_PROHIBITED = 13,
            PR_CONVERTED_EITS = 14,
            PR_DEFERRED_DELIVERY_TIME = 15,
            PR_DELIVER_TIME = 16,
            PR_DISCARD_REASON = 17,
            PR_DISCLOSURE_OF_RECIPIENTS = 18,
            PR_DL_EXPANSION_HISTORY = 19,
            PR_DL_EXPANSION_PROHIBITED = 20,
            PR_EXPIRY_TIME = 21,
            PR_IMPLICIT_CONVERSION_PROHIBITED = 22,
            PR_IMPORTANCE = 23,
            PR_IPM_ID = 24,
            PR_LATEST_DELIVERY_TIME = 25,
            PR_MESSAGE_CLASS = 26,
            PR_MESSAGE_CLASS_A = 26,
            PR_MESSAGE_CLASS_W = 26,
            PR_MESSAGE_DELIVERY_ID = 27,
            PR_MESSAGE_SECURITY_LABEL = 30,
            PR_OBSOLETED_IPMS = 31,
            PR_ORIGINALLY_INTENDED_RECIPIENT_NAME = 32,
            PR_ORIGINAL_EITS = 33,
            PR_ORIGINATOR_CERTIFICATE = 34,
            PR_ORIGINATOR_DELIVERY_REPORT_REQUESTED = 35,
            PR_ORIGINATOR_RETURN_ADDRESS = 36,
            PR_PARENT_KEY = 37,
            PR_PRIORITY = 38,
            PR_ORIGIN_CHECK = 39,
            PR_PROOF_OF_SUBMISSION_REQUESTED = 40,
            PR_READ_RECEIPT_REQUESTED = 41,
            PR_RECEIPT_TIME = 42,
            PR_RECIPIENT_REASSIGNMENT_PROHIBITED = 43,
            PR_REDIRECTION_HISTORY = 44,
            PR_RELATED_IPMS = 45,
            PR_ORIGINAL_SENSITIVITY = 46,
            PR_LANGUAGES = 47,
            PR_LANGUAGES_A = 47,
            PR_LANGUAGES_W = 47,
            PR_REPLY_TIME = 48,
            PR_REPORT_TAG = 49,
            PR_REPORT_TIME = 50,
            PR_RETURNED_IPM = 51,
            PR_SECURITY = 52,
            PR_INCOMPLETE_COPY = 53,
            PR_SENSITIVITY = 54,
            PR_SUBJECT = 55,
            PR_SUBJECT_A = 55,
            PR_SUBJECT_W = 55,
            PR_SUBJECT_IPM = 56,
            PR_CLIENT_SUBMIT_TIME = 57,
            PR_REPORT_NAME = 58,
            PR_REPORT_NAME_A = 58,
            PR_REPORT_NAME_W = 58,
            PR_SENT_REPRESENTING_SEARCH_KEY = 59,
            PR_X400_CONTENT_TYPE = 60,
            PR_SUBJECT_PREFIX = 61,
            PR_SUBJECT_PREFIX_A = 61,
            PR_SUBJECT_PREFIX_W = 61,
            PR_NON_RECEIPT_REASON = 62,
            PR_RECEIVED_BY_ENTRYID = 63,
            PR_RECEIVED_BY_NAME = 64,
            PR_RECEIVED_BY_NAME_A = 64,
            PR_RECEIVED_BY_NAME_W = 64,
            PR_SENT_REPRESENTING_ENTRYID = 65,
            PR_SENT_REPRESENTING_NAME = 66,
            PR_SENT_REPRESENTING_NAME_A = 66,
            PR_SENT_REPRESENTING_NAME_W = 66,
            PR_RCVD_REPRESENTING_ENTRYID = 67,
            PR_RCVD_REPRESENTING_NAME = 68,
            PR_RCVD_REPRESENTING_NAME_A = 68,
            PR_RCVD_REPRESENTING_NAME_W = 68,
            PR_REPORT_ENTRYID = 69,
            PR_READ_RECEIPT_ENTRYID = 70,
            PR_MESSAGE_SUBMISSION_ID = 71,
            PR_PROVIDER_SUBMIT_TIME = 72,
            PR_ORIGINAL_SUBJECT = 73,
            PR_ORIGINAL_SUBJECT_A = 73,
            PR_ORIGINAL_SUBJECT_W = 73,
            PR_DISC_VAL = 74,
            PR_ORIG_MESSAGE_CLASS = 75,
            PR_ORIG_MESSAGE_CLASS_A = 75,
            PR_ORIG_MESSAGE_CLASS_W = 75,
            PR_ORIGINAL_AUTHOR_ENTRYID = 76,
            PR_ORIGINAL_AUTHOR_NAME = 77,
            PR_ORIGINAL_AUTHOR_NAME_A = 77,
            PR_ORIGINAL_AUTHOR_NAME_W = 77,
            PR_ORIGINAL_SUBMIT_TIME = 78,
            PR_REPLY_RECIPIENT_ENTRIES = 79,
            PR_REPLY_RECIPIENT_NAMES = 80,
            PR_REPLY_RECIPIENT_NAMES_A = 80,
            PR_REPLY_RECIPIENT_NAMES_W = 80,
            PR_RECEIVED_BY_SEARCH_KEY = 81,
            PR_RCVD_REPRESENTING_SEARCH_KEY = 82,
            PR_READ_RECEIPT_SEARCH_KEY = 83,
            PR_REPORT_SEARCH_KEY = 84,
            PR_ORIGINAL_DELIVERY_TIME = 85,
            PR_ORIGINAL_AUTHOR_SEARCH_KEY = 86,
            PR_MESSAGE_TO_ME = 87,
            PR_MESSAGE_CC_ME = 88,
            PR_MESSAGE_RECIP_ME = 89,
            PR_ORIGINAL_SENDER_NAME = 90,
            PR_ORIGINAL_SENDER_NAME_A = 90,
            PR_ORIGINAL_SENDER_NAME_W = 90,
            PR_ORIGINAL_SENDER_ENTRYID = 91,
            PR_ORIGINAL_SENDER_SEARCH_KEY = 92,
            PR_ORIGINAL_SENT_REPRESENTING_NAME = 93,
            PR_ORIGINAL_SENT_REPRESENTING_NAME_A = 93,
            PR_ORIGINAL_SENT_REPRESENTING_NAME_W = 93,
            PR_ORIGINAL_SENT_REPRESENTING_ENTRYID = 94,
            PR_ORIGINAL_SENT_REPRESENTING_SEARCH_KEY = 95,
            PR_START_DATE = 96,
            PR_END_DATE = 97,
            PR_OWNER_APPT_ID = 98,
            PR_RESPONSE_REQUESTED = 99,
            PR_SENT_REPRESENTING_ADDRTYPE = 100,
            PR_SENT_REPRESENTING_ADDRTYPE_A = 100,
            PR_SENT_REPRESENTING_ADDRTYPE_W = 100,
            PR_SENT_REPRESENTING_EMAIL_ADDRESS = 101,
            PR_SENT_REPRESENTING_EMAIL_ADDRESS_A = 101,
            PR_SENT_REPRESENTING_EMAIL_ADDRESS_W = 101,
            PR_ORIGINAL_SENDER_ADDRTYPE = 102,
            PR_ORIGINAL_SENDER_ADDRTYPE_A = 102,
            PR_ORIGINAL_SENDER_ADDRTYPE_W = 102,
            PR_ORIGINAL_SENDER_EMAIL_ADDRESS = 103,
            PR_ORIGINAL_SENDER_EMAIL_ADDRESS_A = 103,
            PR_ORIGINAL_SENDER_EMAIL_ADDRESS_W = 103,
            PR_ORIGINAL_SENT_REPRESENTING_ADDRTYPE = 104,
            PR_ORIGINAL_SENT_REPRESENTING_ADDRTYPE_A = 104,
            PR_ORIGINAL_SENT_REPRESENTING_ADDRTYPE_W = 104,
            PR_ORIGINAL_SENT_REPRESENTING_EMAIL_ADDRESS = 105,
            PR_ORIGINAL_SENT_REPRESENTING_EMAIL_ADDRESS_A = 105,
            PR_ORIGINAL_SENT_REPRESENTING_EMAIL_ADDRESS_W = 105,
            PR_CONVERSATION_TOPIC = 112,
            PR_CONVERSATION_TOPIC_A = 112,
            PR_CONVERSATION_TOPIC_W = 112,
            PR_CONVERSATION_INDEX = 113,
            PR_ORIGINAL_DISPLAY_BCC = 114,
            PR_ORIGINAL_DISPLAY_BCC_A = 114,
            PR_ORIGINAL_DISPLAY_BCC_W = 114,
            PR_ORIGINAL_DISPLAY_CC = 115,
            PR_ORIGINAL_DISPLAY_CC_A = 115,
            PR_ORIGINAL_DISPLAY_CC_W = 115,
            PR_ORIGINAL_DISPLAY_TO = 116,
            PR_ORIGINAL_DISPLAY_TO_A = 116,
            PR_ORIGINAL_DISPLAY_TO_W = 116,
            PR_RECEIVED_BY_ADDRTYPE = 117,
            PR_RECEIVED_BY_ADDRTYPE_A = 117,
            PR_RECEIVED_BY_ADDRTYPE_W = 117,
            PR_RECEIVED_BY_EMAIL_ADDRESS = 118,
            PR_RECEIVED_BY_EMAIL_ADDRESS_A = 118,
            PR_RECEIVED_BY_EMAIL_ADDRESS_W = 118,
            PR_RCVD_REPRESENTING_ADDRTYPE = 119,
            PR_RCVD_REPRESENTING_ADDRTYPE_A = 119,
            PR_RCVD_REPRESENTING_ADDRTYPE_W = 119,
            PR_RCVD_REPRESENTING_EMAIL_ADDRESS = 120,
            PR_RCVD_REPRESENTING_EMAIL_ADDRESS_A = 120,
            PR_RCVD_REPRESENTING_EMAIL_ADDRESS_W = 120,
            PR_ORIGINAL_AUTHOR_ADDRTYPE = 121,
            PR_ORIGINAL_AUTHOR_ADDRTYPE_A = 121,
            PR_ORIGINAL_AUTHOR_ADDRTYPE_W = 121,
            PR_ORIGINAL_AUTHOR_EMAIL_ADDRESS = 122,
            PR_ORIGINAL_AUTHOR_EMAIL_ADDRESS_A = 122,
            PR_ORIGINAL_AUTHOR_EMAIL_ADDRESS_W = 122,
            PR_ORIGINALLY_INTENDED_RECIP_ADDRTYPE = 123,
            PR_ORIGINALLY_INTENDED_RECIP_ADDRTYPE_A = 123,
            PR_ORIGINALLY_INTENDED_RECIP_ADDRTYPE_W = 123,
            PR_ORIGINALLY_INTENDED_RECIP_EMAIL_ADDRESS = 124,
            PR_ORIGINALLY_INTENDED_RECIP_EMAIL_ADDRESS_A = 124,
            PR_ORIGINALLY_INTENDED_RECIP_EMAIL_ADDRESS_W = 124,
            PR_CONTENT_INTEGRITY_CHECK = 3072,
            PR_EXPLICIT_CONVERSION = 3073,
            PR_IPM_RETURN_REQUESTED = 3074,
            PR_MESSAGE_TOKEN = 3075,
            PR_NDR_REASON_CODE = 3076,
            PR_NDR_DIAG_CODE = 3077,
            PR_NON_RECEIPT_NOTIFICATION_REQUESTED = 3078,
            PR_DELIVERY_POINT = 3079,
            PR_ORIGINATOR_NON_DELIVERY_REPORT_REQUESTED = 3080,
            PR_ORIGINATOR_REQUESTED_ALTERNATE_RECIPIENT = 3081,
            PR_PHYSICAL_DELIVERY_BUREAU_FAX_DELIVERY = 3082,
            PR_PHYSICAL_DELIVERY_MODE = 3083,
            PR_PHYSICAL_DELIVERY_REPORT_REQUEST = 3084,
            PR_PHYSICAL_FORWARDING_ADDRESS = 3085,
            PR_PHYSICAL_FORWARDING_ADDRESS_REQUESTED = 3086,
            PR_PHYSICAL_FORWARDING_PROHIBITED = 3087,
            PR_PHYSICAL_RENDITION_ATTRIBUTES = 3088,
            PR_PROOF_OF_DELIVERY = 3089,
            PR_PROOF_OF_DELIVERY_REQUESTED = 3090,
            PR_RECIPIENT_CERTIFICATE = 3091,
            PR_RECIPIENT_NUMBER_FOR_ADVICE = 3092,
            PR_RECIPIENT_NUMBER_FOR_ADVICE_A = 3092,
            PR_RECIPIENT_NUMBER_FOR_ADVICE_W = 3092,
            PR_RECIPIENT_TYPE = 3093,
            PR_REGISTERED_MAIL_TYPE = 3094,
            PR_REPLY_REQUESTED = 3095,
            PR_REQUESTED_DELIVERY_METHOD = 3096,
            PR_SENDER_ENTRYID = 3097,
            PR_SENDER_NAME = 3098,
            PR_SENDER_NAME_A = 3098,
            PR_SENDER_NAME_W = 3098,
            PR_SUPPLEMENTARY_INFO = 3099,
            PR_SUPPLEMENTARY_INFO_A = 3099,
            PR_SUPPLEMENTARY_INFO_W = 3099,
            PR_TYPE_OF_MTS_USER = 3100,
            PR_SENDER_SEARCH_KEY = 3101,
            PR_SENDER_ADDRTYPE = 3102,
            PR_SENDER_ADDRTYPE_A = 3102,
            PR_SENDER_ADDRTYPE_W = 3102,
            PR_SENDER_EMAIL_ADDRESS = 3103,
            PR_SENDER_EMAIL_ADDRESS_A = 3103,
            PR_SENDER_EMAIL_ADDRESS_W = 3103,
            PR_CURRENT_VERSION = 3584,
            PR_DELETE_AFTER_SUBMIT = 3585,
            PR_DISPLAY_BCC = 3586,
            PR_DISPLAY_BCC_A = 3586,
            PR_DISPLAY_BCC_W = 3586,
            PR_DISPLAY_CC = 3587,
            PR_DISPLAY_CC_A = 3587,
            PR_DISPLAY_CC_W = 3587,
            PR_DISPLAY_TO = 3588,
            PR_DISPLAY_TO_A = 3588,
            PR_DISPLAY_TO_W = 3588,
            PR_PARENT_DISPLAY = 3589,
            PR_PARENT_DISPLAY_A = 3589,
            PR_PARENT_DISPLAY_W = 3589,
            PR_MESSAGE_DELIVERY_TIME = 3590,
            PR_MESSAGE_FLAGS = 3591,
            PR_MESSAGE_SIZE = 3592,
            PR_PARENT_ENTRYID = 3593,
            PR_SENTMAIL_ENTRYID = 3594,
            PR_CORRELATE = 3596,
            PR_CORRELATE_MTSID = 3597,
            PR_DISCRETE_VALUES = 3598,
            PR_RESPONSIBILITY = 3599,
            PR_SPOOLER_STATUS = 3600,
            PR_TRANSPORT_STATUS = 3601,
            PR_MESSAGE_RECIPIENTS = 3602,
            PR_MESSAGE_ATTACHMENTS = 3603,
            PR_SUBMIT_FLAGS = 3604,
            PR_RECIPIENT_STATUS = 3605,
            PR_TRANSPORT_KEY = 3606,
            PR_MSG_STATUS = 3607,
            PR_MESSAGE_DOWNLOAD_TIME = 3608,
            PR_CREATION_VERSION = 3609,
            PR_MODIFY_VERSION = 3610,
            PR_HASATTACH = 3611,
            PR_BODY_CRC = 3612,
            PR_NORMALIZED_SUBJECT = 3613,
            PR_NORMALIZED_SUBJECT_A = 3613,
            PR_NORMALIZED_SUBJECT_W = 3613,
            PR_RTF_IN_SYNC = 3615,
            PR_ATTACH_SIZE = 3616,
            PR_ATTACH_NUM = 3617,
            PR_PREPROCESS = 3618,
            PR_ORIGINATING_MTA_CERTIFICATE = 3621,
            PR_PROOF_OF_SUBMISSION = 3622,
            PR_PRIMARY_SEND_ACCOUNT = 3624,
            PR_NEXT_SEND_ACCT = 3625,
            PR_ACCESS = 4084,
            PR_ROW_TYPE = 4085,
            PR_INSTANCE_KEY = 4086,
            PR_ACCESS_LEVEL = 4087,
            PR_MAPPING_SIGNATURE = 4088,
            PR_RECORD_KEY = 4089,
            PR_STORE_RECORD_KEY = 4090,
            PR_STORE_ENTRYID = 4091,
            PR_MINI_ICON = 4092,
            PR_ICON = 4093,
            PR_OBJECT_TYPE = 4094,
            PR_ENTRYID = 4095,
            PR_BODY = 4096,
            PR_BODY_A = 4096,
            PR_BODY_W = 4096,
            PR_REPORT_TEXT = 4097,
            PR_REPORT_TEXT_A = 4097,
            PR_REPORT_TEXT_W = 4097,
            PR_ORIGINATOR_AND_DL_EXPANSION_HISTORY = 4098,
            PR_REPORTING_DL_NAME = 4099,
            PR_REPORTING_MTA_CERTIFICATE = 4100,
            PR_RTF_SYNC_BODY_CRC = 4102,
            PR_RTF_SYNC_BODY_COUNT = 4103,
            PR_RTF_SYNC_BODY_TAG = 4104,
            PR_RTF_SYNC_BODY_TAG_A = 4104,
            PR_RTF_SYNC_BODY_TAG_W = 4104,
            PR_RTF_COMPRESSED = 4105,
            PR_RTF_SYNC_PREFIX_COUNT = 4112,
            PR_RTF_SYNC_TRAILING_COUNT = 4113,
            PR_ORIGINALLY_INTENDED_RECIP_ENTRYID = 4114,
            PR_BODY_HTML = 4115,
            PR_ROWID = 12288,
            PR_DISPLAY_NAME = 12289,
            PR_DISPLAY_NAME_A = 12289,
            PR_DISPLAY_NAME_W = 12289,
            PR_ADDRTYPE = 12290,
            PR_ADDRTYPE_A = 12290,
            PR_ADDRTYPE_W = 12290,
            PR_EMAIL_ADDRESS = 12291,
            PR_EMAIL_ADDRESS_A = 12291,
            PR_EMAIL_ADDRESS_W = 12291,
            PR_COMMENT = 12292,
            PR_COMMENT_A = 12292,
            PR_COMMENT_W = 12292,
            PR_DEPTH = 12293,
            PR_PROVIDER_DISPLAY = 12294,
            PR_PROVIDER_DISPLAY_A = 12294,
            PR_PROVIDER_DISPLAY_W = 12294,
            PR_CREATION_TIME = 12295,
            PR_LAST_MODIFICATION_TIME = 12296,
            PR_RESOURCE_FLAGS = 12297,
            PR_PROVIDER_DLL_NAME = 12298,
            PR_PROVIDER_DLL_NAME_A = 12298,
            PR_PROVIDER_DLL_NAME_W = 12298,
            PR_SEARCH_KEY = 12299,
            PR_PROVIDER_UID = 12300,
            PR_PROVIDER_ORDINAL = 12301,
            PR_DEFAULT_STORE = 13312,
            PR_STORE_SUPPORT_MASK = 13325,
            PR_STORE_STATE = 13326,
            PR_IPM_SUBTREE_SEARCH_KEY = 13328,
            PR_IPM_OUTBOX_SEARCH_KEY = 13329,
            PR_IPM_WASTEBASKET_SEARCH_KEY = 13330,
            PR_IPM_SENTMAIL_SEARCH_KEY = 13331,
            PR_MDB_PROVIDER = 13332,
            PR_RECEIVE_FOLDER_SETTINGS = 13333,
            PR_VALID_FOLDER_MASK = 13791,
            PR_IPM_SUBTREE_ENTRYID = 13792,
            PR_IPM_OUTBOX_ENTRYID = 13794,
            PR_IPM_WASTEBASKET_ENTRYID = 13795,
            PR_IPM_SENTMAIL_ENTRYID = 13796,
            PR_VIEWS_ENTRYID = 13797,
            PR_COMMON_VIEWS_ENTRYID = 13798,
            PR_FINDER_ENTRYID = 13799,
            PR_CONTAINER_FLAGS = 13824,
            PR_FOLDER_TYPE = 13825,
            PR_CONTENT_COUNT = 13826,
            PR_CONTENT_UNREAD = 13827,
            PR_CREATE_TEMPLATES = 13828,
            PR_DETAILS_TABLE = 13829,
            PR_SEARCH = 13831,
            PR_SELECTABLE = 13833,
            PR_SUBFOLDERS = 13834,
            PR_STATUS = 13835,
            PR_ANR = 13836,
            PR_ANR_A = 13836,
            PR_ANR_W = 13836,
            PR_CONTENTS_SORT_ORDER = 13837,
            PR_CONTAINER_HIERARCHY = 13838,
            PR_CONTAINER_CONTENTS = 13839,
            PR_FOLDER_ASSOCIATED_CONTENTS = 13840,
            PR_DEF_CREATE_DL = 13841,
            PR_DEF_CREATE_MAILUSER = 13842,
            PR_CONTAINER_CLASS = 13843,
            PR_CONTAINER_CLASS_A = 13843,
            PR_CONTAINER_CLASS_W = 13843,
            PR_CONTAINER_MODIFY_VERSION = 13844,
            PR_AB_PROVIDER_ID = 13845,
            PR_DEFAULT_VIEW_ENTRYID = 13846,
            PR_ASSOC_CONTENT_COUNT = 13847,
            PR_ATTACHMENT_X400_PARAMETERS = 14080,
            PR_ATTACH_DATA_BIN = 14081,
            PR_ATTACH_DATA_OBJ = 14081,
            PR_ATTACH_ENCODING = 14082,
            PR_ATTACH_EXTENSION = 14083,
            PR_ATTACH_EXTENSION_A = 14083,
            PR_ATTACH_EXTENSION_W = 14083,
            PR_ATTACH_FILENAME = 14084,
            PR_ATTACH_FILENAME_A = 14084,
            PR_ATTACH_FILENAME_W = 14084,
            PR_ATTACH_METHOD = 14085,
            PR_ATTACH_LONG_FILENAME = 14087,
            PR_ATTACH_LONG_FILENAME_A = 14087,
            PR_ATTACH_LONG_FILENAME_W = 14087,
            PR_ATTACH_PATHNAME = 14088,
            PR_ATTACH_PATHNAME_A = 14088,
            PR_ATTACH_PATHNAME_W = 14088,
            PR_ATTACH_RENDERING = 14089,
            PR_ATTACH_TAG = 14090,
            PR_RENDERING_POSITION = 14091,
            PR_ATTACH_TRANSPORT_NAME = 14092,
            PR_ATTACH_TRANSPORT_NAME_A = 14092,
            PR_ATTACH_TRANSPORT_NAME_W = 14092,
            PR_ATTACH_LONG_PATHNAME = 14093,
            PR_ATTACH_LONG_PATHNAME_A = 14093,
            PR_ATTACH_LONG_PATHNAME_W = 14093,
            PR_ATTACH_MIME_TAG = 14094,
            PR_ATTACH_MIME_TAG_A = 14094,
            PR_ATTACH_MIME_TAG_W = 14094,
            PR_ATTACH_ADDITIONAL_INFO = 14095,
            PR_ATTACH_CONTENT_ID = 14098,
            PR_ATTACH_CONTENT_LOCATION = 14099,
            PR_DISPLAY_TYPE = 14592,
            PR_TEMPLATEID = 14594,
            PR_PRIMARY_CAPABILITY = 14596,
            PR_7BIT_DISPLAY_NAME = 14847,
            PR_ACCOUNT = 14848,
            PR_ACCOUNT_A = 14848,
            PR_ACCOUNT_W = 14848,
            PR_ALTERNATE_RECIPIENT = 14849,
            PR_CALLBACK_TELEPHONE_NUMBER = 14850,
            PR_CALLBACK_TELEPHONE_NUMBER_A = 14850,
            PR_CALLBACK_TELEPHONE_NUMBER_W = 14850,
            PR_CONVERSION_PROHIBITED = 14851,
            PR_DISCLOSE_RECIPIENTS = 14852,
            PR_GENERATION = 14853,
            PR_GENERATION_A = 14853,
            PR_GENERATION_W = 14853,
            PR_GIVEN_NAME = 14854,
            PR_GIVEN_NAME_A = 14854,
            PR_GIVEN_NAME_W = 14854,
            PR_GOVERNMENT_ID_NUMBER = 14855,
            PR_GOVERNMENT_ID_NUMBER_A = 14855,
            PR_GOVERNMENT_ID_NUMBER_W = 14855,
            PR_BUSINESS_TELEPHONE_NUMBER = 14856,
            PR_BUSINESS_TELEPHONE_NUMBER_A = 14856,
            PR_BUSINESS_TELEPHONE_NUMBER_W = 14856,
            PR_HOME_TELEPHONE_NUMBER = 14857,
            PR_HOME_TELEPHONE_NUMBER_A = 14857,
            PR_HOME_TELEPHONE_NUMBER_W = 14857,
            PR_INITIALS = 14858,
            PR_INITIALS_A = 14858,
            PR_INITIALS_W = 14858,
            PR_KEYWORD = 14859,
            PR_KEYWORD_A = 14859,
            PR_KEYWORD_W = 14859,
            PR_LANGUAGE = 14860,
            PR_LANGUAGE_A = 14860,
            PR_LANGUAGE_W = 14860,
            PR_LOCATION = 14861,
            PR_LOCATION_A = 14861,
            PR_LOCATION_W = 14861,
            PR_MAIL_PERMISSION = 14862,
            PR_MHS_COMMON_NAME = 14863,
            PR_MHS_COMMON_NAME_A = 14863,
            PR_MHS_COMMON_NAME_W = 14863,
            PR_ORGANIZATIONAL_ID_NUMBER = 14864,
            PR_ORGANIZATIONAL_ID_NUMBER_A = 14864,
            PR_ORGANIZATIONAL_ID_NUMBER_W = 14864,
            PR_SURNAME = 14865,
            PR_SURNAME_A = 14865,
            PR_SURNAME_W = 14865,
            PR_ORIGINAL_ENTRYID = 14866,
            PR_ORIGINAL_DISPLAY_NAME = 14867,
            PR_ORIGINAL_DISPLAY_NAME_A = 14867,
            PR_ORIGINAL_DISPLAY_NAME_W = 14867,
            PR_ORIGINAL_SEARCH_KEY = 14868,
            PR_POSTAL_ADDRESS = 14869,
            PR_POSTAL_ADDRESS_A = 14869,
            PR_POSTAL_ADDRESS_W = 14869,
            PR_COMPANY_NAME = 14870,
            PR_COMPANY_NAME_A = 14870,
            PR_COMPANY_NAME_W = 14870,
            PR_TITLE = 14871,
            PR_TITLE_A = 14871,
            PR_TITLE_W = 14871,
            PR_DEPARTMENT_NAME = 14872,
            PR_DEPARTMENT_NAME_A = 14872,
            PR_DEPARTMENT_NAME_W = 14872,
            PR_OFFICE_LOCATION = 14873,
            PR_OFFICE_LOCATION_A = 14873,
            PR_OFFICE_LOCATION_W = 14873,
            PR_PRIMARY_TELEPHONE_NUMBER = 14874,
            PR_PRIMARY_TELEPHONE_NUMBER_A = 14874,
            PR_PRIMARY_TELEPHONE_NUMBER_W = 14874,
            PR_BUSINESS2_TELEPHONE_NUMBER = 14875,
            PR_BUSINESS2_TELEPHONE_NUMBER_A = 14875,
            PR_BUSINESS2_TELEPHONE_NUMBER_W = 14875,
            PR_MOBILE_TELEPHONE_NUMBER = 14876,
            PR_MOBILE_TELEPHONE_NUMBER_A = 14876,
            PR_MOBILE_TELEPHONE_NUMBER_W = 14876,
            PR_RADIO_TELEPHONE_NUMBER = 14877,
            PR_RADIO_TELEPHONE_NUMBER_A = 14877,
            PR_RADIO_TELEPHONE_NUMBER_W = 14877,
            PR_CAR_TELEPHONE_NUMBER = 14878,
            PR_CAR_TELEPHONE_NUMBER_A = 14878,
            PR_CAR_TELEPHONE_NUMBER_W = 14878,
            PR_OTHER_TELEPHONE_NUMBER = 14879,
            PR_OTHER_TELEPHONE_NUMBER_A = 14879,
            PR_OTHER_TELEPHONE_NUMBER_W = 14879,
            PR_TRANSMITABLE_DISPLAY_NAME = 14880,
            PR_TRANSMITABLE_DISPLAY_NAME_A = 14880,
            PR_TRANSMITABLE_DISPLAY_NAME_W = 14880,
            PR_PAGER_TELEPHONE_NUMBER = 14881,
            PR_PAGER_TELEPHONE_NUMBER_A = 14881,
            PR_PAGER_TELEPHONE_NUMBER_W = 14881,
            PR_USER_CERTIFICATE = 14882,
            PR_PRIMARY_FAX_NUMBER = 14883,
            PR_PRIMARY_FAX_NUMBER_A = 14883,
            PR_PRIMARY_FAX_NUMBER_W = 14883,
            PR_BUSINESS_FAX_NUMBER = 14884,
            PR_BUSINESS_FAX_NUMBER_A = 14884,
            PR_BUSINESS_FAX_NUMBER_W = 14884,
            PR_HOME_FAX_NUMBER = 14885,
            PR_HOME_FAX_NUMBER_A = 14885,
            PR_HOME_FAX_NUMBER_W = 14885,
            PR_COUNTRY = 14886,
            PR_COUNTRY_A = 14886,
            PR_COUNTRY_W = 14886,
            PR_LOCALITY = 14887,
            PR_LOCALITY_A = 14887,
            PR_LOCALITY_W = 14887,
            PR_STATE_OR_PROVINCE = 14888,
            PR_STATE_OR_PROVINCE_A = 14888,
            PR_STATE_OR_PROVINCE_W = 14888,
            PR_STREET_ADDRESS = 14889,
            PR_STREET_ADDRESS_A = 14889,
            PR_STREET_ADDRESS_W = 14889,
            PR_POSTAL_CODE = 14890,
            PR_POSTAL_CODE_A = 14890,
            PR_POSTAL_CODE_W = 14890,
            PR_POST_OFFICE_BOX = 14891,
            PR_POST_OFFICE_BOX_A = 14891,
            PR_POST_OFFICE_BOX_W = 14891,
            PR_TELEX_NUMBER = 14892,
            PR_TELEX_NUMBER_A = 14892,
            PR_TELEX_NUMBER_W = 14892,
            PR_ISDN_NUMBER = 14893,
            PR_ISDN_NUMBER_A = 14893,
            PR_ISDN_NUMBER_W = 14893,
            PR_ASSISTANT_TELEPHONE_NUMBER = 14894,
            PR_ASSISTANT_TELEPHONE_NUMBER_A = 14894,
            PR_ASSISTANT_TELEPHONE_NUMBER_W = 14894,
            PR_HOME2_TELEPHONE_NUMBER = 14895,
            PR_HOME2_TELEPHONE_NUMBER_A = 14895,
            PR_HOME2_TELEPHONE_NUMBER_W = 14895,
            PR_ASSISTANT = 14896,
            PR_ASSISTANT_A = 14896,
            PR_ASSISTANT_W = 14896,
            PR_SEND_RICH_INFO = 14912,
            PR_WEDDING_ANNIVERSARY = 14913,
            PR_BIRTHDAY = 14914,
            PR_HOBBIES = 14915,
            PR_HOBBIES_A = 14915,
            PR_HOBBIES_W = 14915,
            PR_MIDDLE_NAME = 14916,
            PR_MIDDLE_NAME_A = 14916,
            PR_MIDDLE_NAME_W = 14916,
            PR_DISPLAY_NAME_PREFIX = 14917,
            PR_DISPLAY_NAME_PREFIX_A = 14917,
            PR_DISPLAY_NAME_PREFIX_W = 14917,
            PR_PROFESSION = 14918,
            PR_PROFESSION_A = 14918,
            PR_PROFESSION_W = 14918,
            PR_PREFERRED_BY_NAME = 14919,
            PR_PREFERRED_BY_NAME_A = 14919,
            PR_PREFERRED_BY_NAME_W = 14919,
            PR_SPOUSE_NAME = 14920,
            PR_SPOUSE_NAME_A = 14920,
            PR_SPOUSE_NAME_W = 14920,
            PR_COMPUTER_NETWORK_NAME = 14921,
            PR_COMPUTER_NETWORK_NAME_A = 14921,
            PR_COMPUTER_NETWORK_NAME_W = 14921,
            PR_CUSTOMER_ID = 14922,
            PR_CUSTOMER_ID_A = 14922,
            PR_CUSTOMER_ID_W = 14922,
            PR_TTYTDD_PHONE_NUMBER = 14923,
            PR_TTYTDD_PHONE_NUMBER_A = 14923,
            PR_TTYTDD_PHONE_NUMBER_W = 14923,
            PR_FTP_SITE = 14924,
            PR_FTP_SITE_A = 14924,
            PR_FTP_SITE_W = 14924,
            PR_GENDER = 14925,
            PR_MANAGER_NAME = 14926,
            PR_MANAGER_NAME_A = 14926,
            PR_MANAGER_NAME_W = 14926,
            PR_NICKNAME = 14927,
            PR_NICKNAME_A = 14927,
            PR_NICKNAME_W = 14927,
            PR_PERSONAL_HOME_PAGE = 14928,
            PR_PERSONAL_HOME_PAGE_A = 14928,
            PR_PERSONAL_HOME_PAGE_W = 14928,
            PR_BUSINESS_HOME_PAGE = 14929,
            PR_BUSINESS_HOME_PAGE_A = 14929,
            PR_BUSINESS_HOME_PAGE_W = 14929,
            PR_CONTACT_VERSION = 14930,
            PR_CONTACT_ENTRYIDS = 14931,
            PR_CONTACT_ADDRTYPES = 14932,
            PR_CONTACT_ADDRTYPES_A = 14932,
            PR_CONTACT_ADDRTYPES_W = 14932,
            PR_CONTACT_DEFAULT_ADDRESS_INDEX = 14933,
            PR_CONTACT_EMAIL_ADDRESSES = 14934,
            PR_CONTACT_EMAIL_ADDRESSES_A = 14934,
            PR_CONTACT_EMAIL_ADDRESSES_W = 14934,
            PR_COMPANY_MAIN_PHONE_NUMBER = 14935,
            PR_COMPANY_MAIN_PHONE_NUMBER_A = 14935,
            PR_COMPANY_MAIN_PHONE_NUMBER_W = 14935,
            PR_CHILDRENS_NAMES = 14936,
            PR_CHILDRENS_NAMES_A = 14936,
            PR_CHILDRENS_NAMES_W = 14936,
            PR_HOME_ADDRESS_CITY = 14937,
            PR_HOME_ADDRESS_CITY_A = 14937,
            PR_HOME_ADDRESS_CITY_W = 14937,
            PR_HOME_ADDRESS_COUNTRY = 14938,
            PR_HOME_ADDRESS_COUNTRY_A = 14938,
            PR_HOME_ADDRESS_COUNTRY_W = 14938,
            PR_HOME_ADDRESS_POSTAL_CODE = 14939,
            PR_HOME_ADDRESS_POSTAL_CODE_A = 14939,
            PR_HOME_ADDRESS_POSTAL_CODE_W = 14939,
            PR_HOME_ADDRESS_STATE_OR_PROVINCE = 14940,
            PR_HOME_ADDRESS_STATE_OR_PROVINCE_A = 14940,
            PR_HOME_ADDRESS_STATE_OR_PROVINCE_W = 14940,
            PR_HOME_ADDRESS_STREET = 14941,
            PR_HOME_ADDRESS_STREET_A = 14941,
            PR_HOME_ADDRESS_STREET_W = 14941,
            PR_HOME_ADDRESS_POST_OFFICE_BOX = 14942,
            PR_HOME_ADDRESS_POST_OFFICE_BOX_A = 14942,
            PR_HOME_ADDRESS_POST_OFFICE_BOX_W = 14942,
            PR_OTHER_ADDRESS_CITY = 14943,
            PR_OTHER_ADDRESS_CITY_A = 14943,
            PR_OTHER_ADDRESS_CITY_W = 14943,
            PR_OTHER_ADDRESS_COUNTRY = 14944,
            PR_OTHER_ADDRESS_COUNTRY_A = 14944,
            PR_OTHER_ADDRESS_COUNTRY_W = 14944,
            PR_OTHER_ADDRESS_POSTAL_CODE = 14945,
            PR_OTHER_ADDRESS_POSTAL_CODE_A = 14945,
            PR_OTHER_ADDRESS_POSTAL_CODE_W = 14945,
            PR_OTHER_ADDRESS_STATE_OR_PROVINCE = 14946,
            PR_OTHER_ADDRESS_STATE_OR_PROVINCE_A = 14946,
            PR_OTHER_ADDRESS_STATE_OR_PROVINCE_W = 14946,
            PR_OTHER_ADDRESS_STREET = 14947,
            PR_OTHER_ADDRESS_STREET_A = 14947,
            PR_OTHER_ADDRESS_STREET_W = 14947,
            PR_OTHER_ADDRESS_POST_OFFICE_BOX = 14948,
            PR_OTHER_ADDRESS_POST_OFFICE_BOX_A = 14948,
            PR_OTHER_ADDRESS_POST_OFFICE_BOX_W = 14948,
            PR_STORE_PROVIDERS = 15616,
            PR_AB_PROVIDERS = 15617,
            PR_TRANSPORT_PROVIDERS = 15618,
            PR_DEFAULT_PROFILE = 15620,
            PR_AB_SEARCH_PATH = 15621,
            PR_AB_DEFAULT_DIR = 15622,
            PR_AB_DEFAULT_PAB = 15623,
            PR_FILTERING_HOOKS = 15624,
            PR_SERVICE_NAME = 15625,
            PR_SERVICE_NAME_A = 15625,
            PR_SERVICE_NAME_W = 15625,
            PR_SERVICE_DLL_NAME = 15626,
            PR_SERVICE_DLL_NAME_A = 15626,
            PR_SERVICE_DLL_NAME_W = 15626,
            PR_SERVICE_ENTRY_NAME = 15627,
            PR_SERVICE_UID = 15628,
            PR_SERVICE_EXTRA_UIDS = 15629,
            PR_SERVICES = 15630,
            PR_SERVICE_SUPPORT_FILES = 15631,
            PR_SERVICE_SUPPORT_FILES_A = 15631,
            PR_SERVICE_SUPPORT_FILES_W = 15631,
            PR_SERVICE_DELETE_FILES = 15632,
            PR_SERVICE_DELETE_FILES_A = 15632,
            PR_SERVICE_DELETE_FILES_W = 15632,
            PR_AB_SEARCH_PATH_UPDATE = 15633,
            PR_PROFILE_NAME = 15634,
            PR_PROFILE_NAME_A = 15634,
            PR_PROFILE_NAME_W = 15634,
            PR_IDENTITY_DISPLAY = 15872,
            PR_IDENTITY_DISPLAY_A = 15872,
            PR_IDENTITY_DISPLAY_W = 15872,
            PR_IDENTITY_ENTRYID = 15873,
            PR_RESOURCE_METHODS = 15874,
            PR_RESOURCE_TYPE = 15875,
            PR_STATUS_CODE = 15876,
            PR_IDENTITY_SEARCH_KEY = 15877,
            PR_OWN_STORE_ENTRYID = 15878,
            PR_RESOURCE_PATH = 15879,
            PR_RESOURCE_PATH_A = 15879,
            PR_RESOURCE_PATH_W = 15879,
            PR_STATUS_STRING = 15880,
            PR_STATUS_STRING_A = 15880,
            PR_STATUS_STRING_W = 15880,
            PR_X400_DEFERRED_DELIVERY_CANCEL = 15881,
            PR_HEADER_FOLDER_ENTRYID = 15882,
            PR_REMOTE_PROGRESS = 15883,
            PR_REMOTE_PROGRESS_TEXT = 15884,
            PR_REMOTE_PROGRESS_TEXT_A = 15884,
            PR_REMOTE_PROGRESS_TEXT_W = 15884,
            PR_REMOTE_VALIDATE_OK = 15885,
            PR_CONTROL_FLAGS = 16128,
            PR_CONTROL_STRUCTURE = 16129,
            PR_CONTROL_TYPE = 16130,
            PR_DELTAX = 16131,
            PR_DELTAY = 16132,
            PR_XPOS = 16133,
            PR_YPOS = 16134,
            PR_CONTROL_ID = 16135,
            PR_INITIAL_DETAILS_PANE = 16136,
            PR_INTERNET_CPID = 16350,
            PR_CREATOR_NAME = 16376,
            PR_MESSAGE_CODEPAGE = 16381,
            PR_SENT_REPRESENTING_SIMPLE_DISPLAY_NAME = 16433,
            PR_OUTLOOK_EMAIL1ADDRTYPE = 32898,
            PR_OUTLOOK_EMAIL1EMAILADDRESS = 32899,
            PR_OUTLOOK_EMAIL1ORIGINALDISPLAYNAME = 32900,
            PR_OUTLOOK_EMAIL1ORIGINALENTRYID = 32901,
            PR_OUTLOOK_EMAIL2ADDRTYPE = 32914,
            PR_OUTLOOK_EMAIL2EMAILADDRESS = 32915,
            PR_OUTLOOK_EMAIL2ORIGINALDISPLAYNAME = 32916,
            PR_OUTLOOK_EMAIL2ORIGINALENTRYID = 32917,
            PR_OUTLOOK_EMAIL3ADDRTYPE = 32930,
            PR_OUTLOOK_EMAIL3EMAILADDRESS = 32931,
            PR_OUTLOOK_EMAIL3ORIGINALDISPLAYNAME = 32932,
            PR_OUTLOOK_EMAIL3ORIGINALENTRYID = 32933,
            PR_OUTLOOK_TASK_COMPLETION_STATUS = 33025,
            PR_OUTLOOK_TASK_PRECENT_COMPLETE = 33026,
            PR_OUTLOOK_TASK_DATE_START = 33028,
            PR_OUTLOOK_TASK_DATE_DUE = 33029,
            PR_OUTLOOK_TASK_IS_NONCURRENT = 33033,
            PR_OUTLOOK_TASK_DATE_COMPLETED = 33039,
            PR_OUTLOOK_TASK_ACTUAL_WORK = 33040,
            PR_OUTLOOK_TASK_TOTAL_WORK = 33041,
            PR_OUTLOOK_TASK_RECURRENCE_SCHEDULE = 33046,
            PR_OUTLOOK_TASK_IS_COMPLETE = 33052,
            PR_OUTLOOK_TASK_IS_RECURRING = 33062,
            PR_OUTLOOK_EVENT_SHOW_TIME_AS = 33285,
            PR_OUTLOOK_EVENT_LOCATION = 33288,
            PR_OUTLOOK_EVENT_START_DATE = 33293,
            PR_OUTLOOK_EVENT_END_DATE = 33294,
            PR_OUTLOOK_EVENT_DURATION = 33299,
            PR_OUTLOOK_EVENT_COLOR = 33300,
            PR_OUTLOOK_EVENT_ALL_DAY = 33301,
            PR_OUTLOOK_EVENT_RECURRENCE_DATA = 33302,
            PR_OUTLOOK_EVENT_IS_RECURRING = 33315,
            PR_OUTLOOK_EVENT_RECURRENCE_BASE = 33320,
            PR_OUTLOOK_EVENT_ORGANIZER = 33326,
            PR_OUTLOOK_EVENT_RECURRENCE_TYPE = 33329,
            PR_OUTLOOK_EVENT_TIMEZONE = 33331,
            PR_OUTLOOK_EVENT_TIMEZONE_DESCRIPTION = 33332,
            PR_OUTLOOK_EVENT_RECURRENCE_START = 33333,
            PR_OUTLOOK_EVENT_RECURRENCE_END = 33334,
            PR_OUTLOOK_ALL_ATTENDEES = 33336,
            PR_OUTLOOK_TO_ATTENDEES = 33339,
            PR_OUTLOOK_CC_ATTENDEES = 33340,
            PR_OUTLOOK_COMMON_REMINDER_MINUTES_BEFORE = 34049,
            PR_OUTLOOK_COMMON_REMINDER_DATE = 34050,
            PR_OUTLOOK_COMMON_REMINDER_SET = 34051,
            PR_OUTLOOK_COMMON_CONTEXT_MENU_FLAGS = 34064,
            PR_OUTLOOK_INTERNAL_VERSION = 34130,
            PR_OUTLOOK_VERSION = 34132,
        }
    }
}