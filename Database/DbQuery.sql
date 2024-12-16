USE [master]
GO
/****** Object:  Database [AiAssistant]    Script Date: 16-12-2024 20:30:43 ******/
CREATE DATABASE [AiAssistant]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AiAssistant', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.TRILOKSQL\MSSQL\DATA\AiAssistant.mdf' , SIZE = 65536KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AiAssistant_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.TRILOKSQL\MSSQL\DATA\AiAssistant_log.ldf' , SIZE = 65536KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [AiAssistant] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AiAssistant].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AiAssistant] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AiAssistant] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AiAssistant] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AiAssistant] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AiAssistant] SET ARITHABORT OFF 
GO
ALTER DATABASE [AiAssistant] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AiAssistant] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AiAssistant] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AiAssistant] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AiAssistant] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AiAssistant] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AiAssistant] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AiAssistant] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AiAssistant] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AiAssistant] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AiAssistant] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AiAssistant] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AiAssistant] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AiAssistant] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AiAssistant] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AiAssistant] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AiAssistant] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AiAssistant] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AiAssistant] SET  MULTI_USER 
GO
ALTER DATABASE [AiAssistant] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AiAssistant] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AiAssistant] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AiAssistant] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AiAssistant] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AiAssistant] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [AiAssistant] SET QUERY_STORE = ON
GO
ALTER DATABASE [AiAssistant] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [AiAssistant]
GO
/****** Object:  Table [dbo].[AIUser]    Script Date: 16-12-2024 20:30:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AIUser](
	[AIUserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](30) NOT NULL,
	[FirstName] [varchar](30) NOT NULL,
	[LastName] [varchar](30) NULL,
	[EmailID] [varchar](50) NOT NULL,
	[PhoneNo] [varchar](15) NOT NULL,
	[Password] [varchar](20) NOT NULL,
	[Gender] [int] NOT NULL,
	[TimeStamp] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[AIUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AIUserInfo]    Script Date: 16-12-2024 20:30:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AIUserInfo](
	[AIUserInfoID] [int] IDENTITY(1,1) NOT NULL,
	[AIUserID] [int] NOT NULL,
	[Name] [varchar](30) NOT NULL,
	[Password] [varchar](30) NOT NULL,
	[Gender] [int] NOT NULL,
	[Latitude] [varchar](30) NOT NULL,
	[FCMToken] [varchar](300) NOT NULL,
	[Longitude] [varchar](30) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AIUserInfoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AIUserLoginInfo]    Script Date: 16-12-2024 20:30:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AIUserLoginInfo](
	[AIUserLoginInfoID] [int] IDENTITY(1,1) NOT NULL,
	[AIUserID] [int] NOT NULL,
	[Latitude] [varchar](30) NOT NULL,
	[Longitude] [varchar](30) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AIUserLoginInfoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AIUserNotification]    Script Date: 16-12-2024 20:30:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AIUserNotification](
	[AIUserNotificationID] [int] IDENTITY(1,1) NOT NULL,
	[AIUserID] [int] NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Message] [varchar](300) NOT NULL,
	[IsNotificationSend] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AIUserNotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AIUser] ADD  DEFAULT ('') FOR [UserName]
GO
ALTER TABLE [dbo].[AIUser] ADD  DEFAULT ('') FOR [FirstName]
GO
ALTER TABLE [dbo].[AIUser] ADD  DEFAULT ('') FOR [LastName]
GO
ALTER TABLE [dbo].[AIUser] ADD  DEFAULT ('') FOR [EmailID]
GO
ALTER TABLE [dbo].[AIUser] ADD  DEFAULT ('') FOR [PhoneNo]
GO
ALTER TABLE [dbo].[AIUserInfo] ADD  DEFAULT ('') FOR [Name]
GO
ALTER TABLE [dbo].[AIUserInfo] ADD  DEFAULT ('') FOR [Latitude]
GO
ALTER TABLE [dbo].[AIUserInfo] ADD  DEFAULT ('') FOR [FCMToken]
GO
ALTER TABLE [dbo].[AIUserInfo] ADD  DEFAULT ('') FOR [Longitude]
GO
ALTER TABLE [dbo].[AIUserInfo] ADD  DEFAULT (getdate()) FOR [TimeStamp]
GO
ALTER TABLE [dbo].[AIUserLoginInfo] ADD  DEFAULT ('') FOR [Latitude]
GO
ALTER TABLE [dbo].[AIUserLoginInfo] ADD  DEFAULT ('') FOR [Longitude]
GO
ALTER TABLE [dbo].[AIUserLoginInfo] ADD  DEFAULT (getdate()) FOR [TimeStamp]
GO
ALTER TABLE [dbo].[AIUserNotification] ADD  DEFAULT (getdate()) FOR [TimeStamp]
GO
ALTER TABLE [dbo].[AIUserNotification] ADD  DEFAULT ('') FOR [Message]
GO
ALTER TABLE [dbo].[AIUserNotification] ADD  DEFAULT ((0)) FOR [IsNotificationSend]
GO
/****** Object:  StoredProcedure [dbo].[usp_AIUser]    Script Date: 16-12-2024 20:30:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_AIUser]  
(  
    @Mode                   INT = 0,  
    @UserName               VARCHAR(30) = '',  
    @FirstName              VARCHAR(20) = '',  
    @LastName               VARCHAR(20) = '',  
    @PhoneNo                VARCHAR(13) = '',  
    @EmailID                VARCHAR(50) = '',  
    @Gender                 INT = 0,  
    @Password               VARCHAR(20) = '',  
    @TimeStamp              DATETIME = ''  
)  
AS  
BEGIN  
    SET NOCOUNT ON;  
      
     
    DECLARE @AIUserID INT;  
      
     
    IF(@Mode = 1)  
    BEGIN  
        
        IF EXISTS (SELECT 1 FROM AIUser WHERE UserName = @UserName)  
        BEGIN  
             
            RAISERROR('The UserName %s is already taken. Please choose a different UserName.', 16, 1, @UserName);  
            RETURN;   
        END  
          
          
        IF EXISTS (SELECT 1 FROM AIUser WHERE EmailID = @EmailID)  
        BEGIN  
            
            RAISERROR('The EmailID %s is already registered. Please use a different EmailID.', 16, 1, @EmailID);  
            RETURN;  -- Exit the procedure  
        END  
          
        -- Insert into AIUser table if UserName and EmailID are not taken  
        INSERT INTO AIUser (UserName, FirstName, LastName, PhoneNo, EmailID, Password, Gender, TimeStamp)  
        VALUES (@UserName, @FirstName, @LastName, @PhoneNo, @EmailID, @Password, @Gender, GETDATE());  
  
        
        SET @AIUserID = SCOPE_IDENTITY();  
          
        SELECT UserName , AIUserID from AIUser WHERE AIUserID = @AIUserID;  
    END 
	IF(@Mode = 2)  
    BEGIN 
        SELECT UserName , AIUserID from AIUser WHERE UserName = @UserName and Password = @Password;  
    END 
END  
GO
/****** Object:  StoredProcedure [dbo].[usp_ChatUser]    Script Date: 16-12-2024 20:30:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[usp_ChatUser]
(
@Mode                  INT = 0,
@UserID                INT = 0
)
AS 
BEGIN
  SET NOCOUNT ON;
  IF(@Mode=1)
  BEGIN
    Select FirstName, LastName, UserName from AIUser where AIUserID = @UserID
  END

END
GO
USE [master]
GO
ALTER DATABASE [AiAssistant] SET  READ_WRITE 
GO
