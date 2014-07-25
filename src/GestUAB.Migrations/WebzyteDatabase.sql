BEGIN TRANSACTION;

CREATE TABLE IF NOT EXISTS [wz_Config]
(
	[DatabaseVersion] VARCHAR(10)
);

INSERT INTO wz_Config (DatabaseVersion) VALUES ('0.2a');

CREATE TABLE IF NOT EXISTS [wz_Downloads]
(
	[DownloadId] GUID NOT NULL,
	[DownloadCategoryId] GUID CONSTRAINT fk_wz_DownloadCategories_DownloadCategoryId REFERENCES wz_DownloadCategories (DownloadCategoryId),
	[Title] VARCHAR(100),
	[Description] VARCHAR(500),
	[Url] VARCHAR(500) NOT NULL ,
	[Version] VARCHAR(20),
	[Release] VARCHAR(20),
	[FileSize] INTEGER,
	[Date] DATETIME NOT NULL DEFAULT 'CURRENT_DATE',
	[DownloadIndex] INTEGER,
    PRIMARY KEY (DownloadId)
);

CREATE UNIQUE INDEX IF NOT EXISTS DownloadId ON [wz_Downloads] (DownloadId);

CREATE TABLE IF NOT EXISTS [wz_DownloadCategories]
(
	[DownloadCategoryId] GUID NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	[Description] VARCHAR(255),
	[DownloadCategoryIndex] INTEGER,
    PRIMARY KEY (DownloadCategoryId)
);

CREATE UNIQUE INDEX IF NOT EXISTS DownloadCategoryId ON [wz_DownloadCategories] (DownloadCategoryId);

CREATE TABLE IF NOT EXISTS [wz_Polls]
(
	[PollId] GUID NOT NULL,
	[Question] VARCHAR(255),
	[Open] BIT DEFAULT 1,
	[Date] DATETIME NOT NULL DEFAULT 'CURRENT_DATE',
    PRIMARY KEY (PollId)
);

CREATE UNIQUE INDEX IF NOT EXISTS PollId ON [wz_Polls] (PollId);

CREATE TABLE IF NOT EXISTS [wz_PollsAnswers]
(
	[PollAnswerId] GUID NOT NULL,
	[PollId] GUID CONSTRAINT fk_wz_PollsAnswers_PollId REFERENCES wz_Polls (PollId),
	[Answer] VARCHAR(200),
	[Votes] INTEGER DEFAULT 0,
    PRIMARY KEY (PollAnswerId)
);

CREATE UNIQUE INDEX IF NOT EXISTS PollAnswerId ON [wz_PollsAnswers] (PollAnswerId);

CREATE TABLE IF NOT EXISTS [wz_NewsCategories]
(
	[NewsCategoryId] GUID NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	[Description] VARCHAR(255),
    PRIMARY KEY (NewsCategoryId)
);
CREATE UNIQUE INDEX IF NOT EXISTS NewsCategoryId ON [wz_NewsCategories] (NewsCategoryId );

CREATE TABLE IF NOT EXISTS [wz_News]
(
	[NewsId] GUID NOT NULL,
	[NewsCategoryId] GUID CONSTRAINT fk_wz_NewsCategories_NewsCategoryId REFERENCES wz_NewsCategories (NewsCategoryId),
	[Title] VARCHAR(200) NOT NULL ,
	[Subtitle] VARCHAR(200),
	[Text] VARCHAR(8000) NOT NULL ,
	[Date] DATETIME NOT NULL DEFAULT 'CURRENT_DATE',
	[Author] VARCHAR(50),
	[Highlight] BIT NOT NULL DEFAULT 0,
    PRIMARY KEY (NewsId)
);
CREATE UNIQUE INDEX IF NOT EXISTS NewsId ON [wz_News] (NewsId);


CREATE TABLE IF NOT EXISTS [wz_NewsletterCategories]
(
	[NewsletterCategoryId] GUID NOT NULL,
	[Name] VARCHAR(50),
	[Description] VARCHAR(255),
    PRIMARY KEY (NewsletterCategoryId)
);

CREATE UNIQUE INDEX IF NOT EXISTS NewsletterCategoryId ON [wz_NewsletterCategories] (NewsletterCategoryId);


CREATE TABLE IF NOT EXISTS [wz_Newsletters]
(
	[NewsletterId] GUID NOT NULL,
	[NewsletterCategoryId] GUID CONSTRAINT fk_wz_NewsletterCategories_NewsletterCategoryId REFERENCES wz_NewsletterCategories (NewsletterCategoryId),
	[Title] VARCHAR(100),
	[Html] VARCHAR(8000) NOT NULL ,
	[Summary] VARCHAR(1000),
	[Date] DATETIME DEFAULT 'CURRENT_DATE',
    PRIMARY KEY (NewsletterId)
);

CREATE UNIQUE INDEX IF NOT EXISTS NewsletterId ON [wz_Newsletters] (NewsletterId);

CREATE TABLE IF NOT EXISTS [wz_NewslettersSubscribers]
(
	[EMail] VARCHAR(100),
	[UserName] VARCHAR(50),
	[Name] VARCHAR(100),
    PRIMARY KEY (EMail, UserName)
);

CREATE TABLE IF NOT EXISTS [wz_SubscribersCategories]
(
	[NewsletterCategoryId] GUID CONSTRAINT fk_wz_SubscribersCategories_NewsletterCategoryId REFERENCES wz_NewsletterCategories (NewsletterCategoryId),
	[Email] VARCHAR(50) CONSTRAINT fk_wz_SubscribersCategories_Email REFERENCES wz_NewslettersSubscribers (EMail),
    PRIMARY KEY (NewsletterCategoryId, Email)
);

CREATE TABLE IF NOT EXISTS [wz_ArticleCategories]
(
	[ArticleCategoryId] GUID NOT NULL,
	[Name] VARCHAR(50),
	[Description] VARCHAR(255),
	PRIMARY KEY (ArticleCategoryId)
);

CREATE UNIQUE INDEX IF NOT EXISTS ArticleCategoryId ON [wz_ArticleCategories] (ArticleCategoryId);

CREATE TABLE IF NOT EXISTS [wz_Articles]
(
	[ArticleId] GUID NOT NULL,
	[ArticleCategoryId] GUID CONSTRAINT fk_wz_ArticleCategories_ArticleCategoryId REFERENCES wz_ArticleCategories (ArticleCategoryId),
	[Title] VARCHAR(100),
	[Subtitle] VARCHAR(100),
	[Author] VARCHAR(100),
	[Summary] VARCHAR(1000),
	[Text] VARCHAR(1000),
	[Image] VARCHAR(100),
	[PublishDate] DATETIME DEFAULT 'CURRENT_DATE',
	PRIMARY KEY (ArticleId)
);

CREATE UNIQUE INDEX IF NOT EXISTS ArticleId ON [wz_Articles] (ArticleId);

/*###################*/

CREATE TABLE IF NOT EXISTS [wz_AdvertisementCategories]
(
	[AdvertisementCategoryId] GUID NOT NULL,
	[Name] VARCHAR(50),
	[Description] VARCHAR(255),
	PRIMARY KEY (AdvertisementCategoryId)
);

CREATE UNIQUE INDEX IF NOT EXISTS AdvertisementCategoryId ON [wz_AdvertisementCategories] (AdvertisementCategoryId);

CREATE TABLE IF NOT EXISTS [wz_Advertisements]
(
	[AdvertisementId] GUID NOT NULL,
	[AdvertisementCategoryId] GUID CONSTRAINT fk_wz_AdvertisementCategories_AdvertisementCategoryId REFERENCES wz_AdvertisementCategories (AdvertisementCategoryId),
	[Title] VARCHAR(100),
	[Text] VARCHAR(1000),
	[Html] VARCHAR(8000),
	[Image] VARCHAR(100),
	PRIMARY KEY (AdvertisementId)
);

CREATE UNIQUE INDEX IF NOT EXISTS AdvertisementId ON [wz_Advertisements] (AdvertisementId);

/*###################*/

CREATE TABLE IF NOT EXISTS [wz_AdvertisingCampaigns]
(
	[AdvertisingCampaignId] GUID NOT NULL,
	[Title] VARCHAR(100),
	[Text] VARCHAR(1000),
	[Image] VARCHAR(100),
	[Price] MONEY DEFAULT 0,
	[Active] BIT DEFAULT 1,
	[PublishDate] DATETIME DEFAULT 'CURRENT_DATE',
	PRIMARY KEY (AdvertisingCampaignId)  
);

CREATE UNIQUE INDEX IF NOT EXISTS AdvertisingCampaignId ON [wz_AdvertisingCampaigns] (AdvertisingCampaignId);


CREATE TABLE IF NOT EXISTS [wz_AdvertisingCampaignsAdvertisements]
(
	[AdvertisingCampaignId] GUID CONSTRAINT fk_wz_AdvertisingCampaigns_AdvertisingCampaignId REFERENCES wz_AdvertisingCampaigns (AdvertisingCampaignId),
	[AdvertisementId] GUID CONSTRAINT fk_wz_Advertisements_AdvertisementId REFERENCES wz_Advertisements (AdvertisementId),
	[Active] BIT DEFAULT 1,
	[PublishDate] DATETIME DEFAULT 'CURRENT_DATE',
	PRIMARY KEY (AdvertisingCampaignId, AdvertisementId)  
);

/*###################*/

CREATE TABLE IF NOT EXISTS [wz_Segments]
(
	[SegmentId] GUID NOT NULL,
	[Name] VARCHAR(100),
	[Description] VARCHAR(500),
	[Image] VARCHAR(100),
	[SegmentIndex] INTEGER DEFAULT 0,
	PRIMARY KEY (SegmentId)  
);
CREATE UNIQUE INDEX IF NOT EXISTS SegmentId ON [wz_Segments] (SegmentId);

CREATE TABLE IF NOT EXISTS [wz_Subsegments]
(
	[SubsegmentId] GUID NOT NULL,
	[SegmentId] GUID CONSTRAINT fk_wz_Segments_SegmentId REFERENCES wz_Segments (SegmentId),
	[Name] VARCHAR(50) NOT NULL,
	[Description] VARCHAR(500),
	[Image] VARCHAR(100),
	[SubsegmentIndex] INTEGER DEFAULT 0,
	PRIMARY KEY (SubsegmentId)  
);
CREATE UNIQUE INDEX IF NOT EXISTS SubsegmentId ON [wz_Subsegments] (SubsegmentId);

/*###################*/

CREATE TABLE IF NOT EXISTS [wz_TipCategories]
(
	[TipCategoryId] GUID NOT NULL,
	[Name] VARCHAR(50),
	[Description] VARCHAR(255),
	PRIMARY KEY (TipCategoryId)  
);

CREATE UNIQUE INDEX IF NOT EXISTS TipCategoryId ON [wz_TipCategories] (TipCategoryId);

CREATE TABLE IF NOT EXISTS [wz_Tips]
(
	[TipId] GUID NOT NULL,
	[TipCategoryId] GUID CONSTRAINT fk_wz_TipCategories_TipCategoryId REFERENCES wz_TipCategories (TipCategoryId),
	[Title] VARCHAR(100),
	[Subtitle] VARCHAR(100),
	[Text] VARCHAR(1000),
	PRIMARY KEY (TipId)
);

CREATE UNIQUE INDEX IF NOT EXISTS TipId ON [wz_Tips] (TipId);

/*###################*/
CREATE TABLE IF NOT EXISTS [wz_FaqTopics]
(
	[FaqTopicId] GUID NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	[Description] VARCHAR(255),
	PRIMARY KEY (FaqTopicId)
);
CREATE UNIQUE INDEX IF NOT EXISTS FaqTopicId ON [wz_FaqTopics] (FaqTopicId);

CREATE TABLE IF NOT EXISTS [wz_Faqs]
(
	[FaqId] GUID NOT NULL,
	[FaqTopicId] GUID CONSTRAINT fk_wz_FaqTopics_FaqTopicId REFERENCES wz_FaqTopics (FaqTopicId),
	[Question] VARCHAR(5000) NOT NULL,
	[Answer] VARCHAR(5000) NOT NULL,
	[FaqIndex] INTEGER DEFAULT 0,
	PRIMARY KEY (FaqId)
);
CREATE UNIQUE INDEX IF NOT EXISTS FaqId ON [wz_Faqs] (FaqId);

-- Drop Trigger
DROP TRIGGER IF EXISTS fki_wz_Downloads_DownloadCategoryId_wz_DownloadCategories_DownloadCategoryId;

-- Foreign Key Preventing insert
CREATE TRIGGER fki_wz_Downloads_DownloadCategoryId_wz_DownloadCategories_DownloadCategoryId
BEFORE INSERT ON [wz_Downloads]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table "wz_Downloads" violates foreign key constraint "fki_wz_Downloads_DownloadCategoryId_wz_DownloadCategories_DownloadCategoryId"')
  WHERE NEW.DownloadCategoryId IS NOT NULL AND (SELECT DownloadCategoryId FROM wz_DownloadCategories WHERE DownloadCategoryId = NEW.DownloadCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fku_wz_Downloads_DownloadCategoryId_wz_DownloadCategories_DownloadCategoryId;

-- Foreign key preventing update
CREATE TRIGGER fku_wz_Downloads_DownloadCategoryId_wz_DownloadCategories_DownloadCategoryId
BEFORE UPDATE ON [wz_Downloads]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table "wz_Downloads" violates foreign key constraint "fku_wz_Downloads_DownloadCategoryId_wz_DownloadCategories_DownloadCategoryId"')
      WHERE NEW.DownloadCategoryId IS NOT NULL AND (SELECT DownloadCategoryId FROM wz_DownloadCategories WHERE DownloadCategoryId = NEW.DownloadCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fkd_wz_Downloads_DownloadCategoryId_wz_DownloadCategories_DownloadCategoryId;

-- Foreign key preventing delete
CREATE TRIGGER fkd_wz_Downloads_DownloadCategoryId_wz_DownloadCategories_DownloadCategoryId
BEFORE DELETE ON wz_DownloadCategories
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table "wz_DownloadCategories" violates foreign key constraint "fkd_wz_Downloads_DownloadCategoryId_wz_DownloadCategories_DownloadCategoryId"')
  WHERE (SELECT DownloadCategoryId FROM wz_Downloads WHERE DownloadCategoryId = OLD.DownloadCategoryId) IS NOT NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fki_wz_PollsAnswers_PollId_wz_Polls_PollId;

-- Foreign Key Preventing insert
CREATE TRIGGER fki_wz_PollsAnswers_PollId_wz_Polls_PollId
BEFORE INSERT ON [wz_PollsAnswers]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table "wz_PollsAnswers" violates foreign key constraint "fki_wz_PollsAnswers_PollId_wz_Polls_PollId"')
  WHERE NEW.PollId IS NOT NULL AND (SELECT PollId FROM wz_Polls WHERE PollId = NEW.PollId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fku_wz_PollsAnswers_PollId_wz_Polls_PollId;

-- Foreign key preventing update
CREATE TRIGGER fku_wz_PollsAnswers_PollId_wz_Polls_PollId
BEFORE UPDATE ON [wz_PollsAnswers]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table "wz_PollsAnswers" violates foreign key constraint "fku_wz_PollsAnswers_PollId_wz_Polls_PollId"')
      WHERE NEW.PollId IS NOT NULL AND (SELECT PollId FROM wz_Polls WHERE PollId = NEW.PollId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fkd_wz_PollsAnswers_PollId_wz_Polls_PollId;

-- Foreign key preventing delete
CREATE TRIGGER fkd_wz_PollsAnswers_PollId_wz_Polls_PollId
BEFORE DELETE ON wz_Polls
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table "wz_Polls" violates foreign key constraint "fkd_wz_PollsAnswers_PollId_wz_Polls_PollId"')
  WHERE (SELECT PollId FROM wz_PollsAnswers WHERE PollId = OLD.PollId) IS NOT NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fki_wz_News_NewsCategoryId_wz_NewsCategories_NewsCategoryId;

-- Foreign Key Preventing insert
CREATE TRIGGER fki_wz_News_NewsCategoryId_wz_NewsCategories_NewsCategoryId
BEFORE INSERT ON [wz_News]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table "wz_News" violates foreign key constraint "fki_wz_News_NewsCategoryId_wz_NewsCategories_NewsCategoryId"')
  WHERE NEW.NewsCategoryId IS NOT NULL AND (SELECT NewsCategoryId FROM wz_NewsCategories WHERE NewsCategoryId = NEW.NewsCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fku_wz_News_NewsCategoryId_wz_NewsCategories_NewsCategoryId;

-- Foreign key preventing update
CREATE TRIGGER fku_wz_News_NewsCategoryId_wz_NewsCategories_NewsCategoryId
BEFORE UPDATE ON [wz_News]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table "wz_News" violates foreign key constraint "fku_wz_News_NewsCategoryId_wz_NewsCategories_NewsCategoryId"')
      WHERE NEW.NewsCategoryId IS NOT NULL AND (SELECT NewsCategoryId FROM wz_NewsCategories WHERE NewsCategoryId = NEW.NewsCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fkd_wz_News_NewsCategoryId_wz_NewsCategories_NewsCategoryId;

-- Foreign key preventing delete
CREATE TRIGGER fkd_wz_News_NewsCategoryId_wz_NewsCategories_NewsCategoryId
BEFORE DELETE ON wz_NewsCategories
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table "wz_NewsCategories" violates foreign key constraint "fkd_wz_News_NewsCategoryId_wz_NewsCategories_NewsCategoryId"')
  WHERE (SELECT NewsCategoryId FROM wz_News WHERE NewsCategoryId = OLD.NewsCategoryId) IS NOT NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fki_wz_Newsletters_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId;

-- Foreign Key Preventing insert
CREATE TRIGGER fki_wz_Newsletters_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId
BEFORE INSERT ON [wz_Newsletters]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table "wz_Newsletters" violates foreign key constraint "fki_wz_Newsletters_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId"')
  WHERE NEW.NewsletterCategoryId IS NOT NULL AND (SELECT NewsletterCategoryId FROM wz_NewsletterCategories WHERE NewsletterCategoryId = NEW.NewsletterCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fku_wz_Newsletters_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId;

-- Foreign key preventing update
CREATE TRIGGER fku_wz_Newsletters_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId
BEFORE UPDATE ON [wz_Newsletters]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table "wz_Newsletters" violates foreign key constraint "fku_wz_Newsletters_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId"')
      WHERE NEW.NewsletterCategoryId IS NOT NULL AND (SELECT NewsletterCategoryId FROM wz_NewsletterCategories WHERE NewsletterCategoryId = NEW.NewsletterCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fkd_wz_Newsletters_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId;

-- Foreign key preventing delete
CREATE TRIGGER fkd_wz_Newsletters_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId
BEFORE DELETE ON wz_NewsletterCategories
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table "wz_NewsletterCategories" violates foreign key constraint "fkd_wz_Newsletters_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId"')
  WHERE (SELECT NewsletterCategoryId FROM wz_Newsletters WHERE NewsletterCategoryId = OLD.NewsletterCategoryId) IS NOT NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fki_wz_SubscribersCategories_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId;

-- Foreign Key Preventing insert
CREATE TRIGGER fki_wz_SubscribersCategories_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId
BEFORE INSERT ON [wz_SubscribersCategories]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table "wz_SubscribersCategories" violates foreign key constraint "fki_wz_SubscribersCategories_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId"')
  WHERE NEW.NewsletterCategoryId IS NOT NULL AND (SELECT NewsletterCategoryId FROM wz_NewsletterCategories WHERE NewsletterCategoryId = NEW.NewsletterCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fku_wz_SubscribersCategories_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId;

-- Foreign key preventing update
CREATE TRIGGER fku_wz_SubscribersCategories_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId
BEFORE UPDATE ON [wz_SubscribersCategories]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table "wz_SubscribersCategories" violates foreign key constraint "fku_wz_SubscribersCategories_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId"')
      WHERE NEW.NewsletterCategoryId IS NOT NULL AND (SELECT NewsletterCategoryId FROM wz_NewsletterCategories WHERE NewsletterCategoryId = NEW.NewsletterCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fkd_wz_SubscribersCategories_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId;

-- Foreign key preventing delete
CREATE TRIGGER fkd_wz_SubscribersCategories_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId
BEFORE DELETE ON wz_NewsletterCategories
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table "wz_NewsletterCategories" violates foreign key constraint "fkd_wz_SubscribersCategories_NewsletterCategoryId_wz_NewsletterCategories_NewsletterCategoryId"')
  WHERE (SELECT NewsletterCategoryId FROM wz_SubscribersCategories WHERE NewsletterCategoryId = OLD.NewsletterCategoryId) IS NOT NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fki_wz_SubscribersCategories_Email_wz_NewslettersSubscribers_EMail;

-- Foreign Key Preventing insert
CREATE TRIGGER fki_wz_SubscribersCategories_Email_wz_NewslettersSubscribers_EMail
BEFORE INSERT ON [wz_SubscribersCategories]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table "wz_SubscribersCategories" violates foreign key constraint "fki_wz_SubscribersCategories_Email_wz_NewslettersSubscribers_EMail"')
  WHERE NEW.Email IS NOT NULL AND (SELECT EMail FROM wz_NewslettersSubscribers WHERE EMail = NEW.Email) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fku_wz_SubscribersCategories_Email_wz_NewslettersSubscribers_EMail;

-- Foreign key preventing update
CREATE TRIGGER fku_wz_SubscribersCategories_Email_wz_NewslettersSubscribers_EMail
BEFORE UPDATE ON [wz_SubscribersCategories]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table "wz_SubscribersCategories" violates foreign key constraint "fku_wz_SubscribersCategories_Email_wz_NewslettersSubscribers_EMail"')
      WHERE NEW.Email IS NOT NULL AND (SELECT EMail FROM wz_NewslettersSubscribers WHERE EMail = NEW.Email) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fkd_wz_SubscribersCategories_Email_wz_NewslettersSubscribers_EMail;

-- Foreign key preventing delete
CREATE TRIGGER fkd_wz_SubscribersCategories_Email_wz_NewslettersSubscribers_EMail
BEFORE DELETE ON wz_NewslettersSubscribers
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table "wz_NewslettersSubscribers" violates foreign key constraint "fkd_wz_SubscribersCategories_Email_wz_NewslettersSubscribers_EMail"')
  WHERE (SELECT Email FROM wz_SubscribersCategories WHERE Email = OLD.EMail) IS NOT NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fki_wz_Articles_ArticleCategoryId_wz_ArticleCategories_ArticleCategoryId;

-- Foreign Key Preventing insert
CREATE TRIGGER fki_wz_Articles_ArticleCategoryId_wz_ArticleCategories_ArticleCategoryId
BEFORE INSERT ON [wz_Articles]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table "wz_Articles" violates foreign key constraint "fki_wz_Articles_ArticleCategoryId_wz_ArticleCategories_ArticleCategoryId"')
  WHERE NEW.ArticleCategoryId IS NOT NULL AND (SELECT ArticleCategoryId FROM wz_ArticleCategories WHERE ArticleCategoryId = NEW.ArticleCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fku_wz_Articles_ArticleCategoryId_wz_ArticleCategories_ArticleCategoryId;

-- Foreign key preventing update
CREATE TRIGGER fku_wz_Articles_ArticleCategoryId_wz_ArticleCategories_ArticleCategoryId
BEFORE UPDATE ON [wz_Articles]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table "wz_Articles" violates foreign key constraint "fku_wz_Articles_ArticleCategoryId_wz_ArticleCategories_ArticleCategoryId"')
      WHERE NEW.ArticleCategoryId IS NOT NULL AND (SELECT ArticleCategoryId FROM wz_ArticleCategories WHERE ArticleCategoryId = NEW.ArticleCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fkd_wz_Articles_ArticleCategoryId_wz_ArticleCategories_ArticleCategoryId;

-- Foreign key preventing delete
CREATE TRIGGER fkd_wz_Articles_ArticleCategoryId_wz_ArticleCategories_ArticleCategoryId
BEFORE DELETE ON wz_ArticleCategories
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table "wz_ArticleCategories" violates foreign key constraint "fkd_wz_Articles_ArticleCategoryId_wz_ArticleCategories_ArticleCategoryId"')
  WHERE (SELECT ArticleCategoryId FROM wz_Articles WHERE ArticleCategoryId = OLD.ArticleCategoryId) IS NOT NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fki_wz_Advertisements_AdvertisementCategoryId_wz_AdvertisementCategories_AdvertisementCategoryId;

-- Foreign Key Preventing insert
CREATE TRIGGER fki_wz_Advertisements_AdvertisementCategoryId_wz_AdvertisementCategories_AdvertisementCategoryId
BEFORE INSERT ON [wz_Advertisements]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table "wz_Advertisements" violates foreign key constraint "fki_wz_Advertisements_AdvertisementCategoryId_wz_AdvertisementCategories_AdvertisementCategoryId"')
  WHERE NEW.AdvertisementCategoryId IS NOT NULL AND (SELECT AdvertisementCategoryId FROM wz_AdvertisementCategories WHERE AdvertisementCategoryId = NEW.AdvertisementCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fku_wz_Advertisements_AdvertisementCategoryId_wz_AdvertisementCategories_AdvertisementCategoryId;

-- Foreign key preventing update
CREATE TRIGGER fku_wz_Advertisements_AdvertisementCategoryId_wz_AdvertisementCategories_AdvertisementCategoryId
BEFORE UPDATE ON [wz_Advertisements]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table "wz_Advertisements" violates foreign key constraint "fku_wz_Advertisements_AdvertisementCategoryId_wz_AdvertisementCategories_AdvertisementCategoryId"')
      WHERE NEW.AdvertisementCategoryId IS NOT NULL AND (SELECT AdvertisementCategoryId FROM wz_AdvertisementCategories WHERE AdvertisementCategoryId = NEW.AdvertisementCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fkd_wz_Advertisements_AdvertisementCategoryId_wz_AdvertisementCategories_AdvertisementCategoryId;

-- Foreign key preventing delete
CREATE TRIGGER fkd_wz_Advertisements_AdvertisementCategoryId_wz_AdvertisementCategories_AdvertisementCategoryId
BEFORE DELETE ON wz_AdvertisementCategories
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table "wz_AdvertisementCategories" violates foreign key constraint "fkd_wz_Advertisements_AdvertisementCategoryId_wz_AdvertisementCategories_AdvertisementCategoryId"')
  WHERE (SELECT AdvertisementCategoryId FROM wz_Advertisements WHERE AdvertisementCategoryId = OLD.AdvertisementCategoryId) IS NOT NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fki_wz_Subsegments_SegmentId_wz_Segments_SegmentId;

-- Foreign Key Preventing insert
CREATE TRIGGER fki_wz_Subsegments_SegmentId_wz_Segments_SegmentId
BEFORE INSERT ON [wz_Subsegments]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table "wz_Subsegments" violates foreign key constraint "fki_wz_Subsegments_SegmentId_wz_Segments_SegmentId"')
  WHERE NEW.SegmentId IS NOT NULL AND (SELECT SegmentId FROM wz_Segments WHERE SegmentId = NEW.SegmentId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fku_wz_Subsegments_SegmentId_wz_Segments_SegmentId;

-- Foreign key preventing update
CREATE TRIGGER fku_wz_Subsegments_SegmentId_wz_Segments_SegmentId
BEFORE UPDATE ON [wz_Subsegments]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table "wz_Subsegments" violates foreign key constraint "fku_wz_Subsegments_SegmentId_wz_Segments_SegmentId"')
      WHERE NEW.SegmentId IS NOT NULL AND (SELECT SegmentId FROM wz_Segments WHERE SegmentId = NEW.SegmentId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fkd_wz_Subsegments_SegmentId_wz_Segments_SegmentId;

-- Foreign key preventing delete
CREATE TRIGGER fkd_wz_Subsegments_SegmentId_wz_Segments_SegmentId
BEFORE DELETE ON wz_Segments
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table "wz_Segments" violates foreign key constraint "fkd_wz_Subsegments_SegmentId_wz_Segments_SegmentId"')
  WHERE (SELECT SegmentId FROM wz_Subsegments WHERE SegmentId = OLD.SegmentId) IS NOT NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fki_wz_Tips_TipCategoryId_wz_TipCategories_TipCategoryId;

-- Foreign Key Preventing insert
CREATE TRIGGER fki_wz_Tips_TipCategoryId_wz_TipCategories_TipCategoryId
BEFORE INSERT ON [wz_Tips]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table "wz_Tips" violates foreign key constraint "fki_wz_Tips_TipCategoryId_wz_TipCategories_TipCategoryId"')
  WHERE NEW.TipCategoryId IS NOT NULL AND (SELECT TipCategoryId FROM wz_TipCategories WHERE TipCategoryId = NEW.TipCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fku_wz_Tips_TipCategoryId_wz_TipCategories_TipCategoryId;

-- Foreign key preventing update
CREATE TRIGGER fku_wz_Tips_TipCategoryId_wz_TipCategories_TipCategoryId
BEFORE UPDATE ON [wz_Tips]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table "wz_Tips" violates foreign key constraint "fku_wz_Tips_TipCategoryId_wz_TipCategories_TipCategoryId"')
      WHERE NEW.TipCategoryId IS NOT NULL AND (SELECT TipCategoryId FROM wz_TipCategories WHERE TipCategoryId = NEW.TipCategoryId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fkd_wz_Tips_TipCategoryId_wz_TipCategories_TipCategoryId;

-- Foreign key preventing delete
CREATE TRIGGER fkd_wz_Tips_TipCategoryId_wz_TipCategories_TipCategoryId
BEFORE DELETE ON wz_TipCategories
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table "wz_TipCategories" violates foreign key constraint "fkd_wz_Tips_TipCategoryId_wz_TipCategories_TipCategoryId"')
  WHERE (SELECT TipCategoryId FROM wz_Tips WHERE TipCategoryId = OLD.TipCategoryId) IS NOT NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fki_wz_Faqs_FaqTopicId_wz_FaqTopics_FaqTopicId;

-- Foreign Key Preventing insert
CREATE TRIGGER fki_wz_Faqs_FaqTopicId_wz_FaqTopics_FaqTopicId
BEFORE INSERT ON [wz_Faqs]
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'insert on table "wz_Faqs" violates foreign key constraint "fki_wz_Faqs_FaqTopicId_wz_FaqTopics_FaqTopicId"')
  WHERE NEW.FaqTopicId IS NOT NULL AND (SELECT FaqTopicId FROM wz_FaqTopics WHERE FaqTopicId = NEW.FaqTopicId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fku_wz_Faqs_FaqTopicId_wz_FaqTopics_FaqTopicId;

-- Foreign key preventing update
CREATE TRIGGER fku_wz_Faqs_FaqTopicId_wz_FaqTopics_FaqTopicId
BEFORE UPDATE ON [wz_Faqs]
FOR EACH ROW BEGIN
    SELECT RAISE(ROLLBACK, 'update on table "wz_Faqs" violates foreign key constraint "fku_wz_Faqs_FaqTopicId_wz_FaqTopics_FaqTopicId"')
      WHERE NEW.FaqTopicId IS NOT NULL AND (SELECT FaqTopicId FROM wz_FaqTopics WHERE FaqTopicId = NEW.FaqTopicId) IS NULL;
END;

-- Drop Trigger
DROP TRIGGER IF EXISTS fkd_wz_Faqs_FaqTopicId_wz_FaqTopics_FaqTopicId;

-- Foreign key preventing delete
CREATE TRIGGER fkd_wz_Faqs_FaqTopicId_wz_FaqTopics_FaqTopicId
BEFORE DELETE ON wz_FaqTopics
FOR EACH ROW BEGIN
  SELECT RAISE(ROLLBACK, 'delete on table "wz_FaqTopics" violates foreign key constraint "fkd_wz_Faqs_FaqTopicId_wz_FaqTopics_FaqTopicId"')
  WHERE (SELECT FaqTopicId FROM wz_Faqs WHERE FaqTopicId = OLD.FaqTopicId) IS NOT NULL;
END;

COMMIT TRANSACTION;