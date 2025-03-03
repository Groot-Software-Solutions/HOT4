CREATE VIEW [dbo].[vwSMS]
	AS SELECT s.*,d.Direction,st.Status FROM SMS s
	inner join Direction d on s.DirectionId = d.Id
	inner join Status st on s.StatusId =  st.Id

