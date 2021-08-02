-- MySQL dump 10.13  Distrib 8.0.25, for Win64 (x86_64)
--
-- Host: localhost    Database: samokatcity
-- ------------------------------------------------------
-- Server version	8.0.25

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `ordertransports`
--

DROP TABLE IF EXISTS `ordertransports`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ordertransports` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Order_Id` int DEFAULT NULL,
  `Transport_Id` int DEFAULT NULL,
  `Rates_Id` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_OrderTransports_Orders_Order_Id` (`Order_Id`),
  KEY `FK_OrderTransports_Transports_Transport_Id` (`Transport_Id`),
  KEY `FK_OrderTransports_Rates_Rates_Id` (`Rates_Id`),
  CONSTRAINT `FK_OrderTransports_Orders_Order_Id` FOREIGN KEY (`Order_Id`) REFERENCES `orders` (`Id`),
  CONSTRAINT `FK_OrderTransports_Rates_Rates_Id` FOREIGN KEY (`Rates_Id`) REFERENCES `rates` (`Id`),
  CONSTRAINT `FK_OrderTransports_Transports_Transport_Id` FOREIGN KEY (`Transport_Id`) REFERENCES `transports` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=64 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ordertransports`
--

LOCK TABLES `ordertransports` WRITE;
/*!40000 ALTER TABLE `ordertransports` DISABLE KEYS */;
INSERT INTO `ordertransports` VALUES (40,33,8,10),(49,34,8,7),(51,41,8,10),(52,41,11,10),(55,35,8,8),(56,35,11,8),(57,43,8,9),(58,43,11,9),(59,43,12,9),(60,43,15,9),(61,43,17,9),(62,43,18,9),(63,43,19,9);
/*!40000 ALTER TABLE `ordertransports` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-08-01 11:42:35
