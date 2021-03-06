USE [King-Market]
GO

--ClassDocumentTypes
INSERT [dbo].[ClassDocumentTypes] ([Name]) VALUES (N'Identificacion')
INSERT [dbo].[ClassDocumentTypes] ([Name]) VALUES (N'Pago')
INSERT [dbo].[ClassDocumentTypes] ([Name]) VALUES (N'Traslado')
GO

--DocumentTypes
INSERT [dbo].[DocumentTypes] ([Name], [OnlyForEnterprise], [ClassDocumentTypeId]) VALUES (N'DNI', 0, 1)
INSERT [dbo].[DocumentTypes] ([Name], [OnlyForEnterprise], [ClassDocumentTypeId]) VALUES (N'RUC', 1, 1)
INSERT [dbo].[DocumentTypes] ([Name], [OnlyForEnterprise], [ClassDocumentTypeId]) VALUES (N'Factura', 1, 2)
INSERT [dbo].[DocumentTypes] ([Name], [OnlyForEnterprise], [ClassDocumentTypeId]) VALUES (N'Boleta', 0, 2)
INSERT [dbo].[DocumentTypes] ([Name], [OnlyForEnterprise], [ClassDocumentTypeId]) VALUES (N'Guia', 0, 3)
GO

--EmployeeTypes
INSERT [dbo].[EmployeeTypes] ([Name]) VALUES (N'Almacenero')
INSERT [dbo].[EmployeeTypes] ([Name]) VALUES (N'Comprador')
GO

--ProductTypes
INSERT [dbo].[ProductTypes] ([Name]) VALUES (N'Celulares')
INSERT [dbo].[ProductTypes] ([Name]) VALUES (N'Computadoras')
INSERT [dbo].[ProductTypes] ([Name]) VALUES (N'Laptops')
INSERT [dbo].[ProductTypes] ([Name]) VALUES (N'Smart Watches')
INSERT [dbo].[ProductTypes] ([Name]) VALUES (N'Streaming Box')
INSERT [dbo].[ProductTypes] ([Name]) VALUES (N'Tablets')
GO

--Status
INSERT [dbo].[Status] ([Name]) VALUES (N'Abierto')
INSERT [dbo].[Status] ([Name]) VALUES (N'Activo')
INSERT [dbo].[Status] ([Name]) VALUES (N'Anulado')
INSERT [dbo].[Status] ([Name]) VALUES (N'Cerrado')
INSERT [dbo].[Status] ([Name]) VALUES (N'Inactivo')
GO

--UnitMeasures
INSERT [dbo].[UnitMeasures] ([ShortName], [Name]) VALUES (N'UND', N'Unidad')
INSERT [dbo].[UnitMeasures] ([ShortName], [Name]) VALUES (N'PZA', N'Pieza')
GO
