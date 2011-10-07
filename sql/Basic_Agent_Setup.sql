INSERT INTO [Destination] ([ID],[WriteURL],[ReadURL])
     VALUES (1,'C:\Geckon\Octopus\plugins\','C:\Geckon\Octopus\plugins\')

DECLARE @AgentID			INT
DECLARE @ImagePlugin		INT
DECLARE @TranscodePlugin	INT
DECLARE @NetPlugin			INT
DECLARE @FilePlugin			INT
DECLARE @FileSlot			INT
DECLARE @TranscodeSlot		INT
DECLARE @ImagingSlot		INT  

INSERT INTO [Agent] ([DateCreated])
     VALUES (GETDATE())
     
SET @AgentID = @@IDENTITY   

INSERT INTO [ExecutionSlot] ([AgentID],[MaxSlots],[DateCreated])
     VALUES (@AgentID,2,GETDATE())
     
SET @FileSlot = @@IDENTITY
     
INSERT INTO [ExecutionSlot] ([AgentID],[MaxSlots],[DateCreated])
     VALUES (@AgentID,4,GETDATE())
     
SET @TranscodeSlot = @@IDENTITY
     
INSERT INTO [ExecutionSlot] ([AgentID],[MaxSlots],[DateCreated])
     VALUES (@AgentID,10,GETDATE())
     
SET @ImagingSlot = @@IDENTITY
     
INSERT INTO [Assembly] ([DestinationID],[Version],[Name],[Filename],[CreatedDate])
     VALUES (1,'1.0.0.0','Geckon.Octopus.Plugins.Transcoding.Image','Geckon.Octopus.Plugins.Transcoding.Image.dll',GETDATE())
     
SET @ImagePlugin = @@IDENTITY
     
INSERT INTO [Assembly] ([DestinationID],[Version],[Name],[Filename],[CreatedDate])
     VALUES (1,'1.0.0.0','Geckon.Octopus.Plugins.Transcoding.FFmpeg','Geckon.Octopus.Plugins.Transcoding.FFmpeg.dll',GETDATE())

SET @TranscodePlugin = @@IDENTITY
     
INSERT INTO [Assembly] ([DestinationID],[Version],[Name],[Filename],[CreatedDate])
     VALUES (1,'1.0.0.0','Geckon.Octopus.Plugins.FilePlugins','Geckon.Octopus.Plugins.FilePlugins.dll',GETDATE())

SET @FilePlugin = @@IDENTITY

INSERT INTO [Assembly] ([DestinationID],[Version],[Name],[Filename],[CreatedDate])
     VALUES (1,'1.0.0.0','Geckon.Octopus.Plugins.Net.HttpRequest','Geckon.Octopus.Plugins.Net.HttpRequest.dll',GETDATE())

SET @NetPlugin = @@IDENTITY

INSERT INTO [Plugin] ([AssemblyID],[Name],[Description],[Classname],[CreatedDate])
     VALUES (@ImagePlugin,'Geckon Image Resize Plugin','This plugin resizes images, using .Net','ImageResizePlugin',GETDATE())
     
INSERT INTO [ExecutionSlot_Plugin_Join] ([ExecutionSlotID],[PluginID])
     VALUES (@ImagingSlot,@@IDENTITY)
 
INSERT INTO [Plugin] ([AssemblyID],[Name],[Description],[Classname],[CreatedDate])
     VALUES (@TranscodePlugin,'Geckon FFmpeg Create Still Plugin','This plugin uses the Geckon FFmpeg wrapper to grab a still from a video','FFmpegCreateStillPlugin',GETDATE())

INSERT INTO [ExecutionSlot_Plugin_Join] ([ExecutionSlotID],[PluginID])
     VALUES (@ImagingSlot,@@IDENTITY)
     
INSERT INTO [Plugin] ([AssemblyID],[Name],[Description],[Classname],[CreatedDate])
     VALUES (@TranscodePlugin,'Geckon Transcode TwoPass h264 Plugin','This plugin uses the Geckon FFmpeg wrapper to transcode a video to H264 high quality','TranscodeTwoPassh264Plugin',GETDATE())

INSERT INTO [ExecutionSlot_Plugin_Join] ([ExecutionSlotID],[PluginID])
     VALUES (@TranscodeSlot,@@IDENTITY)

INSERT INTO [Plugin] ([AssemblyID],[Name],[Description],[Classname],[CreatedDate])
     VALUES (@FilePlugin,'Geckon File Copy Plugin','This plugin copies a file','FileCopyPlugin',GETDATE())

INSERT INTO [ExecutionSlot_Plugin_Join] ([ExecutionSlotID],[PluginID])
     VALUES (@FileSlot,@@IDENTITY)

INSERT INTO [Plugin] ([AssemblyID],[Name],[Description],[Classname],[CreatedDate])
     VALUES (@FilePlugin,'Geckon File Delete Plugin','This plugin deletes a file','FileDeletePlugin',GETDATE())
     
INSERT INTO [ExecutionSlot_Plugin_Join] ([ExecutionSlotID],[PluginID])
     VALUES (@FileSlot,@@IDENTITY)
     
     INSERT INTO [Plugin] ([AssemblyID],[Name],[Description],[Classname],[CreatedDate])
     VALUES (@NetPlugin,'Geckon HttpRequest Plugin','This plugin sends a Http Request','HttpRequestPlugin',GETDATE())
     
INSERT INTO [ExecutionSlot_Plugin_Join] ([ExecutionSlotID],[PluginID])
     VALUES (@ImagingSlot,@@IDENTITY)