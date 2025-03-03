INSERT INTO [dbo].[tblTemplate] ([TemplateID],[TemplateName],[TemplateText])
     VALUES
           (700,'Network Timeout','%NETWORK% recharge platform webservice timed out.'),
		   (701,'Network Connection Issue','%NETWORK% recharge platform connection issue.'),
		     (702,'Network Webservice Unavailable','%NETWORK% recharge platform webservice unavailable.'),
			   (703,'Network Webservice Error','%NETWORK% recharge platform webservice Internal Error'),
			     (704,'Network General Error','%NETWORK% general platorm webservice error.');

INSERT INTO [dbo].[tblTemplate] ([TemplateID],[TemplateName],[TemplateText])
     VALUES
           (700,'Failed Web Recharge Min/Max','Your recharge request was too much or too little. Minimum Recharge %MIN% Maximum Recharge %MAX%, try in the correct range. HOT Recharge.');

