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
-- Table structure for table `promotionstransportmodels`
--

DROP TABLE IF EXISTS `promotionstransportmodels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `promotionstransportmodels` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Promotions_Id` int DEFAULT NULL,
  `TransportModels_Id` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_PromotionsTransportModels_Promotions_Promotions_Id` (`Promotions_Id`),
  KEY `FK_45adc646665e435d9268dcaba019e601` (`TransportModels_Id`),
  CONSTRAINT `FK_45adc646665e435d9268dcaba019e601` FOREIGN KEY (`TransportModels_Id`) REFERENCES `transportmodels` (`Id`),
  CONSTRAINT `FK_PromotionsTransportModels_Promotions_Promotions_Id` FOREIGN KEY (`Promotions_Id`) REFERENCES `promotions` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `promotionstransportmodels`
--

LOCK TABLES `promotionstransportmodels` WRITE;
/*!40000 ALTER TABLE `promotionstransportmodels` DISABLE KEYS */;
INSERT INTO `promotionstransportmodels` VALUES (12,7,10),(13,7,11),(14,7,12),(15,6,12);
/*!40000 ALTER TABLE `promotionstransportmodels` ENABLE KEYS */;
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
