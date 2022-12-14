USE [HOC_Kashikiri]
GO
ALTER TABLE
    [TKD_SeiPrS]
ADD
    [TenantCdSeq] INT NULL
GO
UPDATE
    [TKD_SeiPrS]
SET
    [TenantCdSeq] = 1
ALTER TABLE
    [TKD_SeiPrS]
ALTER COLUMN
    [TenantCdSeq] INT NOT NULL
GO
ALTER TABLE
    [TKD_SeiPrS] DROP CONSTRAINT [PK_TKD_SeiPrS]
GO
ALTER TABLE
    [TKD_SeiPrS]
ADD
    CONSTRAINT [PK_TKD_SeiPrS] PRIMARY KEY CLUSTERED ([SeiOutSeq] ASC, [TenantCdSeq] ASC)
GO

--------
USE [HOC_Kashikiri]
GO

ALTER TABLE
    [TKD_Seikyu]
ADD
    [TenantCdSeq] INT NULL, 
        [SeiFileId] VARCHAR(255) NULL
GO
UPDATE
    [TKD_Seikyu]
SET
    [TenantCdSeq] = 1,
        [SeiFileId] = ''

ALTER TABLE
    [TKD_Seikyu]
ALTER COLUMN
    [TenantCdSeq] INT NOT NULL
ALTER TABLE
    [TKD_Seikyu]
ALTER COLUMN
    [SeiFileId] VARCHAR(255) NOT NULL
GO

ALTER TABLE
    [TKD_Seikyu] DROP CONSTRAINT [TKD_Seikyu1]
GO
ALTER TABLE
    [TKD_Seikyu]
ADD
    CONSTRAINT [TKD_Seikyu1] PRIMARY KEY CLUSTERED (
        [SeiOutSeq] ASC,
        [TenantCdSeq] ASC,
        [SeiRen] ASC
    )
GO

-------
USE [HOC_Kashikiri]
GO
ALTER TABLE
    [TKD_SeiMei]
ADD
    [TenantCdSeq] INT NULL
GO
UPDATE
    [TKD_SeiMei]
SET
    [TenantCdSeq] = 1
ALTER TABLE
    [TKD_SeiMei]
ALTER COLUMN
    [TenantCdSeq] INT NOT NULL
GO
ALTER TABLE
    [TKD_SeiMei] DROP CONSTRAINT [TKD_SeiMei1]
GO
ALTER TABLE
    [TKD_SeiMei]
ADD
    CONSTRAINT [TKD_SeiMei1] PRIMARY KEY CLUSTERED (
        [SeiOutSeq] ASC,
        [TenantCdSeq] ASC,
        [SeiRen] ASC,
        [SeiMeiRen] ASC
    )
GO

------
USE [HOC_Kashikiri]
GO
ALTER TABLE
    [TKD_SeiUch]
ADD
    [TenantCdSeq] INT NULL
GO
UPDATE
    [TKD_SeiUch]
SET
    [TenantCdSeq] = 1
ALTER TABLE
    [TKD_SeiUch]
ALTER COLUMN
    [TenantCdSeq] INT NOT NULL
GO
ALTER TABLE
    [TKD_SeiUch] DROP CONSTRAINT [TKD_SeiUch1]
GO
ALTER TABLE
    [TKD_SeiUch]
ADD
    CONSTRAINT [TKD_SeiUch1] PRIMARY KEY CLUSTERED (
        [SeiOutSeq] ASC,
        [TenantCdSeq] ASC,
        [SeiRen] ASC,
        [SeiMeiRen] ASC,
        [SeiUchRen] ASC
    )
GO