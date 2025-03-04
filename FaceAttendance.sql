CREATE DATABASE [FaceAttendance]
USE [FaceAttendance]
GO
/****** Object:  Table [dbo].[Attendances]    Script Date: 3/25/2021 3:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attendances](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_subject] [int] NULL,
	[id_student] [int] NULL,
	[checkin] [datetime] NULL,
	[checkout] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Keys]    Script Date: 3/25/2021 3:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Keys](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[keyText] [varchar](30) NOT NULL,
	[isUsed] [bit] NULL,
	[key_type] [varchar](30) NULL,
	[create_date] [datetime] NULL,
	[expiry_date] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Majors]    Script Date: 3/25/2021 3:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Majors](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[school_id] [int] NULL,
	[startDate] [datetime] NULL,
	[name] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchoolAccounts]    Script Date: 3/25/2021 3:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchoolAccounts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[username] [nvarchar](30) NULL,
	[password] [nvarchar](30) NULL,
	[external_id] [varchar](100) NULL,
	[key_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentImages]    Script Date: 3/25/2021 3:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentImages](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[url] [nvarchar](255) NOT NULL,
	[student_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 3/25/2021 3:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[date_of_birth] [date] NULL,
	[school_id] [int] NULL,
	[image_url] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subjects]    Script Date: 3/25/2021 3:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subjects](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NULL,
	[room] [nvarchar](100) NOT NULL,
	[major_id] [int] NOT NULL,
	[teacher_observer] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeacherAccounts]    Script Date: 3/25/2021 3:41:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeacherAccounts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[email] [nvarchar](30) NULL,
	[password] [nvarchar](30) NULL,
	[external_id] [varchar](100) NULL,
	[teach_in_school] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Attendances] ON 

INSERT [dbo].[Attendances] ([id], [id_subject], [id_student], [checkin], [checkout]) VALUES (5, 2, 2, CAST(N'2000-01-01T00:00:00.000' AS DateTime), CAST(N'2000-01-01T00:00:00.000' AS DateTime))
INSERT [dbo].[Attendances] ([id], [id_subject], [id_student], [checkin], [checkout]) VALUES (6, 2, 3, CAST(N'2000-01-01T00:00:00.000' AS DateTime), CAST(N'2000-01-01T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Attendances] OFF
GO
SET IDENTITY_INSERT [dbo].[Keys] ON 

INSERT [dbo].[Keys] ([id], [keyText], [isUsed], [key_type], [create_date], [expiry_date]) VALUES (1, N'xN9bFcqDRiTv6Qc8aLU3TKR1u3gfX9', 1, N'school_key', CAST(N'2021-03-25T00:00:00.000' AS DateTime), CAST(N'2021-04-04T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Keys] OFF
GO
SET IDENTITY_INSERT [dbo].[Majors] ON 

INSERT [dbo].[Majors] ([id], [school_id], [startDate], [name]) VALUES (2, 1, CAST(N'2021-05-02T15:09:00.000' AS DateTime), N'Thi học sinh dở 30/4')
SET IDENTITY_INSERT [dbo].[Majors] OFF
GO
SET IDENTITY_INSERT [dbo].[SchoolAccounts] ON 

INSERT [dbo].[SchoolAccounts] ([id], [name], [username], [password], [external_id], [key_id]) VALUES (1, N'THPT Thực hành sư phạm', N'admin', N'admin', NULL, NULL)
SET IDENTITY_INSERT [dbo].[SchoolAccounts] OFF
GO
SET IDENTITY_INSERT [dbo].[Students] ON 

INSERT [dbo].[Students] ([id], [name], [date_of_birth], [school_id], [image_url]) VALUES (2, N'Lê Quốc Đạt 09', CAST(N'2000-02-10' AS Date), 1, N'624787389a61683f3170213034439.jpg')
INSERT [dbo].[Students] ([id], [name], [date_of_birth], [school_id], [image_url]) VALUES (3, N'Khang', CAST(N'2000-02-14' AS Date), 1, N'WIN_20210325_11_00_04_Pro210137113.jpg')
SET IDENTITY_INSERT [dbo].[Students] OFF
GO
SET IDENTITY_INSERT [dbo].[Subjects] ON 

INSERT [dbo].[Subjects] ([id], [name], [room], [major_id], [teacher_observer]) VALUES (2, N'Tin học dở', N'P01', 2, 1)
SET IDENTITY_INSERT [dbo].[Subjects] OFF
GO
SET IDENTITY_INSERT [dbo].[TeacherAccounts] ON 

INSERT [dbo].[TeacherAccounts] ([id], [email], [password], [external_id], [teach_in_school]) VALUES (1, N'khangzxrr@gmail.com', N'123123aaa', NULL, 1)
SET IDENTITY_INSERT [dbo].[TeacherAccounts] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Keys__C3D71DAA66FA8206]    Script Date: 3/25/2021 3:41:04 PM ******/
ALTER TABLE [dbo].[Keys] ADD UNIQUE NONCLUSTERED 
(
	[keyText] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__SchoolAc__F3DBC572BAC0D268]    Script Date: 3/25/2021 3:41:04 PM ******/
ALTER TABLE [dbo].[SchoolAccounts] ADD UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__TeacherA__AB6E61648361220B]    Script Date: 3/25/2021 3:41:04 PM ******/
ALTER TABLE [dbo].[TeacherAccounts] ADD UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Keys] ADD  DEFAULT ((0)) FOR [isUsed]
GO
ALTER TABLE [dbo].[Keys] ADD  DEFAULT (getdate()) FOR [create_date]
GO
ALTER TABLE [dbo].[Attendances]  WITH CHECK ADD FOREIGN KEY([id_student])
REFERENCES [dbo].[Students] ([id])
GO
ALTER TABLE [dbo].[Attendances]  WITH CHECK ADD FOREIGN KEY([id_subject])
REFERENCES [dbo].[Subjects] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Majors]  WITH CHECK ADD FOREIGN KEY([school_id])
REFERENCES [dbo].[SchoolAccounts] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SchoolAccounts]  WITH CHECK ADD FOREIGN KEY([key_id])
REFERENCES [dbo].[Keys] ([id])
GO
ALTER TABLE [dbo].[StudentImages]  WITH CHECK ADD FOREIGN KEY([student_id])
REFERENCES [dbo].[Students] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Students]  WITH CHECK ADD FOREIGN KEY([school_id])
REFERENCES [dbo].[SchoolAccounts] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subjects]  WITH CHECK ADD FOREIGN KEY([major_id])
REFERENCES [dbo].[Majors] ([id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Subjects]  WITH CHECK ADD FOREIGN KEY([teacher_observer])
REFERENCES [dbo].[TeacherAccounts] ([id])
GO
ALTER TABLE [dbo].[TeacherAccounts]  WITH CHECK ADD FOREIGN KEY([teach_in_school])
REFERENCES [dbo].[SchoolAccounts] ([id])
ON DELETE CASCADE
GO
