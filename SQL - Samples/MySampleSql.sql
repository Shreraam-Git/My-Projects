
---------------------------------------------------------Stored Procedures---------------------------------------------------------

--Inserting the error msg from the front end and getting the error definition to show in the front end
ALTER PROCEDURE [dbo].[ERROR_DATA]
    @FormName VARCHAR(MAX), @ErrorMessage VARCHAR(MAX), @UserName VARCHAR(MAX)
AS
Begin
	--Inserting Error Message
	INSERT INTO ERROR_LOG(CATCH_ERROR_MSG, FORM_NAME, ERROR_DEFINITION, UPDATED_BY) 
	VALUES (@ErrorMessage, @FormName, (SELECT DISTINCT TOP 1 ERROR_DEFINITION FROM ERROR_LOG WHERE CATCH_ERROR_MSG = @ErrorMessage), @UserName)

	--Getting Error Definition (If Available)
	SELECT DISTINCT ISNULL(ERROR_DEFINITION, '0') AS [ERROR_DEFINITION] FROM ERROR_LOG WHERE CATCH_ERROR_MSG = @ErrorMessage
End


--Getting the points based on there entries on the front end
ALTER PROCEDURE [dbo].[FORMULA_ONE_POINTS_CONDITION]
    @Year VARCHAR(MAX),
    @Round VARCHAR(MAX),
    @UserId VARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        DriverData.YourEntry AS [YOUR_ENTRY_D],
        CASE WHEN DriverData.YourEntryImg IS NULL OR DriverData.YourEntryImg = '' 
             THEN '../assets/images/Formula1/DriversImages/images.png'
             ELSE DriverData.YourEntryImg 
        END AS [YOUR_ENTRY_IMG_D],
        DriverData.RaceResult AS [RACE_RESULT_D],
        CASE WHEN DriverData.RaceResultImg IS NULL OR DriverData.RaceResultImg = '' 
             THEN '../assets/images/Formula1/DriversImages/images.png'
             ELSE DriverData.RaceResultImg 
        END AS [RACE_RESULT_IMG_D],
        CASE WHEN DriverData.YourEntry = DriverData.RaceResult 
             THEN dbo.GetDirectPoints('Drivers - Exact Finishing Order', DriverData.DriverPos)
             ELSE '0' 
        END AS [POINTS_D],

        CASE WHEN DriverData.YourEntry = DriverData.RaceResult
             THEN dbo.GetConditionBasedPoints('ATG', @Year, @Round, DriverData.DriverField, DriverData.YourEntry, DriverData.DriverPos)
             ELSE '0' 
        END AS [AGAINST_THE_GRAIN_D],

        -- Random order condition (only once)
        CASE WHEN DriverData.DriverPos = '1' 
             THEN dbo.GetConditionBasedPoints('ROC', @Year, @Round, '', @UserId, '') 
             ELSE '' 
        END AS [DRIVER_RANDOM_ORDER],

        -- Sprint Driver (similar to above)
        SprintData.YourEntry AS [YOUR_ENTRY_SD],
        CASE WHEN SprintData.YourEntryImg IS NULL OR SprintData.YourEntryImg = '' 
             THEN '../assets/images/Formula1/ConstructorsImage/images.png'
             ELSE SprintData.YourEntryImg 
        END AS [YOUR_ENTRY_IMG_SD],
        SprintData.RaceResult AS [RACE_RESULT_SD],
        CASE WHEN SprintData.RaceResultImg IS NULL OR SprintData.RaceResultImg = '' 
             THEN '../assets/images/Formula1/ConstructorsImage/images.png'
             ELSE SprintData.RaceResultImg 
        END AS [RACE_RESULT_IMG_SD],
        CASE WHEN SprintData.YourEntry = SprintData.RaceResult
             THEN dbo.GetDirectPoints('Sprint - Exact Finishing Order', DriverData.DriverPos)
             ELSE '0' 
        END AS [POINTS_SD],

        -- Constructor
        ConstructorData.YourEntry AS [YOUR_ENTRY_C],
        CASE WHEN ConstructorData.YourEntryImg IS NULL OR ConstructorData.YourEntryImg = '' 
             THEN '../assets/images/Formula1/ConstructorsImage/images.png'
             ELSE ConstructorData.YourEntryImg 
        END AS [YOUR_ENTRY_IMG_C],
        ConstructorData.RaceResult AS [RACE_RESULT_C],
        CASE WHEN ConstructorData.RaceResultImg IS NULL OR ConstructorData.RaceResultImg = '' 
             THEN '../assets/images/Formula1/ConstructorsImage/images.png'
             ELSE ConstructorData.RaceResultImg 
        END AS [RACE_RESULT_IMG_C],
        CASE WHEN ConstructorData.YourEntry = ConstructorData.RaceResult
             THEN dbo.GetDirectPoints('Constructors - Exact Finishing Order', DriverData.DriverPos)
             ELSE '0' 
        END AS [POINTS_C],
        CASE WHEN DriverData.DriverPos = '1' 
             THEN dbo.GetConditionBasedPoints('CROC', @Year, @Round, '', @UserId, '') 
             ELSE '' 
        END AS [CONSTRUCTOR_RANDOM_ORDER],

        -- Lap details (only once when DriverPos = '1')
        CASE WHEN DriverData.DriverPos = '1' THEN A.POLE_POSITION ELSE '' END AS [YOUR_ENTRY_PP],
        CASE WHEN DriverData.DriverPos = '1' AND (A.POLE_POSITION_IMG IS NULL OR A.POLE_POSITION_IMG = '')
             THEN '../assets/images/Formula1/ConstructorsImage/images.png'
             ELSE A.POLE_POSITION_IMG 
        END AS [YOUR_ENTRY_IMG_PP],
        CASE WHEN DriverData.DriverPos = '1' THEN B.POLE_POSITION ELSE '' END AS [RACE_RESULT_PP],
        CASE WHEN DriverData.DriverPos = '1' AND (B.POLE_POSITION_IMG IS NULL OR B.POLE_POSITION_IMG = '')
             THEN '../assets/images/Formula1/ConstructorsImage/images.png'
             ELSE B.POLE_POSITION_IMG 
        END AS [RACE_RESULT_IMG_PP],
        CASE WHEN DriverData.DriverPos = '1' AND A.POLE_POSITION = B.POLE_POSITION 
             THEN dbo.GetDirectPoints('Lap-Details', 'PP')
             ELSE '0'
        END AS [POINTS_PP],
        CASE WHEN DriverData.DriverPos = '1' AND A.POLE_POSITION = B.POLE_POSITION 
             THEN dbo.GetConditionBasedPoints('PPATG', @Year, @Round, 'POLE_POSITION', A.POLE_POSITION, '1')
             ELSE '0'
        END AS [AGAINST_THE_GRAIN_PP],
        CASE WHEN DriverData.DriverPos = '1' THEN A.FASTEST_LAP ELSE '' END AS [YOUR_ENTRY_FL],
        CASE WHEN DriverData.DriverPos = '1' AND (A.FASTEST_LAP_IMG IS NULL OR A.FASTEST_LAP_IMG = '')
             THEN '../assets/images/Formula1/ConstructorsImage/images.png'
             ELSE A.FASTEST_LAP_IMG 
        END AS [YOUR_ENTRY_IMG_FL],
        CASE WHEN DriverData.DriverPos = '1' THEN B.FASTEST_LAP ELSE '' END AS [RACE_RESULT_FL],
        CASE WHEN DriverData.DriverPos = '1' AND (B.FASTEST_LAP_IMG IS NULL OR B.FASTEST_LAP_IMG = '')
             THEN '../assets/images/Formula1/ConstructorsImage/images.png'
             ELSE B.FASTEST_LAP_IMG 
        END AS [RACE_RESULT_IMG_FL],
        CASE WHEN DriverData.DriverPos = '1' AND A.FASTEST_LAP = B.FASTEST_LAP 
             THEN dbo.GetDirectPoints('Lap-Details', 'FL')
             ELSE '0'
        END AS [POINTS_FL],
        CASE WHEN DriverData.DriverPos = '1' THEN A.MOST_PLACE_GAINED ELSE '' END AS [YOUR_ENTRY_MPG],
        CASE WHEN DriverData.DriverPos = '1' AND (A.MOST_PLACE_GAINED_IMG IS NULL OR A.MOST_PLACE_GAINED_IMG = '')
             THEN '../assets/images/Formula1/ConstructorsImage/images.png'
             ELSE A.MOST_PLACE_GAINED_IMG 
        END AS [YOUR_ENTRY_IMG_MPG],
        CASE WHEN DriverData.DriverPos = '1' THEN B.MOST_PLACE_GAINED ELSE '' END AS [RACE_RESULT_MPG],
        CASE WHEN DriverData.DriverPos = '1' AND (B.MOST_PLACE_GAINED_IMG IS NULL OR B.MOST_PLACE_GAINED_IMG = '')
             THEN '../assets/images/Formula1/ConstructorsImage/images.png'
             ELSE B.MOST_PLACE_GAINED_IMG 
        END AS [RACE_RESULT_IMG_MPG],
        CASE WHEN DriverData.DriverPos = '1' AND A.MOST_PLACE_GAINED = B.MOST_PLACE_GAINED 
             THEN dbo.GetDirectPoints('Lap-Details', 'MPG')
             ELSE '0'
        END AS [POINTS_MPG]
    FROM 
        F1_RACE_ENTRY A
        LEFT JOIN F1_MASTER_RACE_RESULT B 
            ON A.YEAR = B.YEAR AND A.ROUND = B.ROUND AND A.STATUS = B.STATUS
        CROSS APPLY 
        (VALUES 
            ('1', A.DRIVER_1, A.DRIVER_1_IMG, B.DRIVER_1, B.DRIVER_1_IMG, 'DRIVER_1'),
            ('2', A.DRIVER_2, A.DRIVER_2_IMG, B.DRIVER_2, B.DRIVER_2_IMG, 'DRIVER_2'),
            ('3', A.DRIVER_3, A.DRIVER_3_IMG, B.DRIVER_3, B.DRIVER_3_IMG, 'DRIVER_3'),
            ('4', A.DRIVER_4, A.DRIVER_4_IMG, B.DRIVER_4, B.DRIVER_4_IMG, 'DRIVER_4')
        ) AS DriverData (DriverPos, YourEntry, YourEntryImg, RaceResult, RaceResultImg, DriverField)
        CROSS APPLY 
        (VALUES 
            (A.SPRINT_DRIVER_1, A.SPRINT_DRIVER_1_IMG, B.SPRINT_DRIVER_1, B.SPRINT_DRIVER_1_IMG),
            (A.SPRINT_DRIVER_2, A.SPRINT_DRIVER_2_IMG, B.SPRINT_DRIVER_2, B.SPRINT_DRIVER_2_IMG),
            (A.SPRINT_DRIVER_3, A.SPRINT_DRIVER_3_IMG, B.SPRINT_DRIVER_3, B.SPRINT_DRIVER_3_IMG),
            (A.SPRINT_DRIVER_4, A.SPRINT_DRIVER_4_IMG, B.SPRINT_DRIVER_4, B.SPRINT_DRIVER_4_IMG)
        ) AS SprintData (YourEntry, YourEntryImg, RaceResult, RaceResultImg)
        CROSS APPLY 
        (VALUES 
            (A.CONSTRUCTOR_1, A.CONSTRUCTOR_1_IMG, B.CONSTRUCTOR_1, B.CONSTRUCTOR_1_IMG),
            (A.CONSTRUCTOR_2, A.CONSTRUCTOR_2_IMG, B.CONSTRUCTOR_2, B.CONSTRUCTOR_2_IMG),
            (A.CONSTRUCTOR_3, A.CONSTRUCTOR_3_IMG, B.CONSTRUCTOR_3, B.CONSTRUCTOR_3_IMG),
            (A.CONSTRUCTOR_4, A.CONSTRUCTOR_4_IMG, B.CONSTRUCTOR_4, B.CONSTRUCTOR_4_IMG)
        ) AS ConstructorData (YourEntry, YourEntryImg, RaceResult, RaceResultImg)
    WHERE 
        A.YEAR = @Year AND 
        A.ROUND = @Round AND 
        A.UPDATED_BY = @UserId;
END

---------------------------------------------------------Stored Procedures---------------------------------------------------------

-------------------------------------------------------------Functions-------------------------------------------------------------

--Getting the ponits based on there conditions - called on the above procedure
ALTER FUNCTION [dbo].[GetConditionBasedPoints] (
	@Type VARCHAR(MAX),
    @Year VARCHAR(MAX),
    @Round VARCHAR(MAX),
	@ColumnName VARCHAR(MAX),
	@Value VARCHAR(MAX),
	@Points VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @Result VARCHAR(MAX);
	DECLARE @Count INT;		

	IF @Type = 'ATG' --Against the Grain For Drivers
	BEGIN
		--Driver--
		IF @ColumnName = 'DRIVER_1'	
		SELECT @Count = COUNT(*) FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND DRIVER_1 = @Value AND STATUS = 'Active';
		IF @ColumnName = 'DRIVER_2'
		SELECT @Count = COUNT(*) FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND DRIVER_2 = @Value AND STATUS = 'Active';
		IF @ColumnName = 'DRIVER_3'
		SELECT @Count = COUNT(*) FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND DRIVER_3 = @Value AND STATUS = 'Active';
		IF @ColumnName = 'DRIVER_4'
		SELECT @Count = COUNT(*) FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND DRIVER_4 = @Value AND STATUS = 'Active';
		IF @ColumnName = 'DRIVER_5'
		SELECT @Count = COUNT(*) FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND DRIVER_5 = @Value AND STATUS = 'Active';
		IF @ColumnName = 'DRIVER_6'
		SELECT @Count = COUNT(*) FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND DRIVER_6 = @Value AND STATUS = 'Active';
		--Driver--	

		--Condition--
		IF @Count <= 5
		SELECT @Result = dbo.GetDirectPoints('Drivers - Against the Grain', @Points)
		ELSE 
		SET @Result = '0'
		--Condition--
	END

	IF @Type = 'PPATG' --Against the Grain For FL Drivers
	BEGIN
		--Driver--
		IF @ColumnName = 'POLE_POSITION'	
		SELECT @Count = COUNT(*) FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND POLE_POSITION = @Value AND STATUS = 'Active';		
		--Driver--	

		--Condition--
		IF @Count <= 5
		SELECT @Result = dbo.GetDirectPoints('Pole Position - Against the Grain', @Points)
		ELSE 
		SET @Result = '0'
		--Condition--
	END

	IF @Type = 'ROC' --Random Order Condition For Drivers
	BEGIN
		SELECT @Count = SUM([Count]) FROM
		(
			SELECT COUNT(*) AS [Count] FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND UPDATED_BY = @Value AND DRIVER_1 IN 
			(
				SELECT Driver
				FROM F1_MASTER_RACE_RESULT
				UNPIVOT
				(
					Driver FOR DriverColumn IN (DRIVER_1, DRIVER_2, DRIVER_3, DRIVER_4, DRIVER_5, DRIVER_6)
				) AS UnpivotedDrivers
				WHERE YEAR = @Year AND ROUND = @Round
			)
			UNION ALL
			SELECT COUNT(*) AS [Count] FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND UPDATED_BY = @Value AND DRIVER_2 IN 
			(
				SELECT Driver
				FROM F1_MASTER_RACE_RESULT
				UNPIVOT
				(
					Driver FOR DriverColumn IN (DRIVER_1, DRIVER_2, DRIVER_3, DRIVER_4, DRIVER_5, DRIVER_6)
				) AS UnpivotedDrivers
				WHERE YEAR = @Year AND ROUND = @Round
			)
			UNION ALL
			SELECT COUNT(*) AS [Count] FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND UPDATED_BY = @Value AND DRIVER_3 IN 
			(
				SELECT Driver
				FROM F1_MASTER_RACE_RESULT
				UNPIVOT
				(
					Driver FOR DriverColumn IN (DRIVER_1, DRIVER_2, DRIVER_3, DRIVER_4, DRIVER_5, DRIVER_6)
				) AS UnpivotedDrivers
				WHERE YEAR = @Year AND ROUND = @Round
			)
			UNION ALL
			SELECT COUNT(*) AS [Count] FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND UPDATED_BY = @Value AND DRIVER_4 IN 
			(
				SELECT Driver
				FROM F1_MASTER_RACE_RESULT
				UNPIVOT
				(
					Driver FOR DriverColumn IN (DRIVER_1, DRIVER_2, DRIVER_3, DRIVER_4, DRIVER_5, DRIVER_6)
				) AS UnpivotedDrivers
				WHERE YEAR = @Year AND ROUND = @Round
			)
			UNION ALL
			SELECT COUNT(*) AS [Count] FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND UPDATED_BY = @Value AND DRIVER_5 IN 
			(
				SELECT Driver
				FROM F1_MASTER_RACE_RESULT
				UNPIVOT
				(
					Driver FOR DriverColumn IN (DRIVER_1, DRIVER_2, DRIVER_3, DRIVER_4, DRIVER_5, DRIVER_6)
				) AS UnpivotedDrivers
				WHERE YEAR = @Year AND ROUND = @Round
			)
			UNION ALL
			SELECT COUNT(*) AS [Count] FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND UPDATED_BY = @Value AND DRIVER_6 IN 
			(
				SELECT Driver
				FROM F1_MASTER_RACE_RESULT
				UNPIVOT
				(
					Driver FOR DriverColumn IN (DRIVER_1, DRIVER_2, DRIVER_3, DRIVER_4, DRIVER_5, DRIVER_6)
				) AS UnpivotedDrivers
				WHERE YEAR = @Year AND ROUND = @Round
			)
		) AS AllCounts

		--Result
		SET @Result = @Count
	END

	IF @Type = 'CROC' --Random Order Condition For Constructors
	BEGIN
		SELECT @Count = SUM([Count]) FROM
		(
			SELECT COUNT(*) AS [Count] FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND UPDATED_BY = @Value AND CONSTRUCTOR_1 IN 
			(
				SELECT Constructor
				FROM F1_MASTER_RACE_RESULT
				UNPIVOT
				(
					Constructor FOR ConstructorColumn IN (CONSTRUCTOR_1, CONSTRUCTOR_2, CONSTRUCTOR_3)
				) AS UnpivotedConstructors
				WHERE YEAR = @Year AND ROUND = @Round
			)
			UNION ALL
			SELECT COUNT(*) AS [Count] FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND UPDATED_BY = @Value AND CONSTRUCTOR_2 IN 
			(
				SELECT Constructor
				FROM F1_MASTER_RACE_RESULT
				UNPIVOT
				(
					Constructor FOR ConstructorColumn IN (CONSTRUCTOR_1, CONSTRUCTOR_2, CONSTRUCTOR_3)
				) AS UnpivotedConstructors
				WHERE YEAR = @Year AND ROUND = @Round
			)
			UNION ALL
			SELECT COUNT(*) AS [Count] FROM F1_RACE_ENTRY WHERE YEAR = @Year AND ROUND = @Round AND UPDATED_BY = @Value AND CONSTRUCTOR_3 IN 
			(
				SELECT Constructor
				FROM F1_MASTER_RACE_RESULT
				UNPIVOT
				(
					Constructor FOR ConstructorColumn IN (CONSTRUCTOR_1, CONSTRUCTOR_2, CONSTRUCTOR_3)
				) AS UnpivotedConstructors
				WHERE YEAR = @Year AND ROUND = @Round
			)			
		) AS AllCounts

		--Result
		SET @Result = @Count
	END

    RETURN @Result;
END;

--Getting the direct ponits based on there conditions - called on the above procedure
ALTER FUNCTION [dbo].[GetDirectPoints] (
    @Criteria VARCHAR(MAX),
    @PointsCategory VARCHAR(MAX)
)
RETURNS VARCHAR(MAX)
AS
BEGIN
    DECLARE @Result VARCHAR(MAX);

    SELECT
        @Result = 
        CASE
            WHEN @PointsCategory = '1' THEN POINTS_1ST
			WHEN @PointsCategory = '2' THEN POINTS_2ND
			WHEN @PointsCategory = '3' THEN POINTS_3RD
			WHEN @PointsCategory = '4' THEN POINTS_4TH
			WHEN @PointsCategory = '5' THEN POINTS_5TH
			WHEN @PointsCategory = '6' THEN POINTS_6TH
			WHEN @PointsCategory = 'Won' THEN WON
			WHEN @PointsCategory = 'Loss' THEN LOSS
			WHEN @PointsCategory = 'Tie' THEN TIE
			WHEN @PointsCategory = 'FL' THEN FL
			WHEN @PointsCategory = 'MPG' THEN MPG
			WHEN @PointsCategory = 'PP' THEN PP
        END
    FROM F1_MASTER_POINTS
    WHERE CRITERIA = @Criteria AND STATUS = 'Active';

    RETURN @Result;
END;

-------------------------------------------------------------Functions-------------------------------------------------------------