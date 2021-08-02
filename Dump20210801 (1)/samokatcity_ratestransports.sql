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
-- Table structure for table `ratestransports`
--

DROP TABLE IF EXISTS `ratestransports`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ratestransports` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Rates_Id` int DEFAULT NULL,
  `TransportModels_Id` int DEFAULT NULL,
  `Price` decimal(18,2) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_RatesTransports_Rates_Rates_Id` (`Rates_Id`),
  KEY `FK_RatesTransports_TransportModels_TransportModels_Id` (`TransportModels_Id`),
  CONSTRAINT `FK_RatesTransports_Rates_Rates_Id` FOREIGN KEY (`Rates_Id`) REFERENCES `rates` (`Id`),
  CONSTRAINT `FK_RatesTransports_TransportModels_TransportModels_Id` FOREIGN KEY (`TransportModels_Id`) REFERENCES `transportmodels` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ratestransports`
--

LOCK TABLES `ratestransports` WRITE;
/*!40000 ALTER TABLE `ratestransports` DISABLE KEYS */;
INSERT INTO `ratestransports` VALUES (7,5,10,600.00),(8,5,11,800.00),(9,5,12,900.00),(10,6,10,800.00),(11,6,11,1000.00),(12,6,12,1100.00),(13,7,10,900.00),(14,7,11,1200.00),(15,7,12,1300.00),(16,8,10,1500.00),(17,8,11,2000.00),(18,8,12,2300.00),(19,9,10,1200.00),(20,9,11,1700.00),(21,9,12,1900.00),(22,10,10,400.00),(23,10,11,550.00),(24,10,12,650.00),(25,11,10,2100.00),(26,11,11,2700.00),(27,11,12,3300.00);
/*!40000 ALTER TABLE `ratestransports` ENABLE KEYS */;
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
