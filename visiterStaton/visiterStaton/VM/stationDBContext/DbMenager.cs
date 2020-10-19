using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace visiterStaton
{
    class DbMenager
    {
        static string createSqlString = @"

-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
-- -----------------------------------------------------
-- Schema df
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema df
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `df` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci ;
-- -----------------------------------------------------
-- Schema station
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema station
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `station` DEFAULT CHARACTER SET utf8 ;
USE `df` ;

-- -----------------------------------------------------
-- Table `df`.`trailer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `df`.`trailer` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `number` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

USE `station` ;

-- -----------------------------------------------------
-- Table `station`.`access_ level`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`access_ level` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `access_ level` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`car`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`car` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `model` VARCHAR(45) NOT NULL,
  `number` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`department`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`department` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`employee`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`employee` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `lastName` VARCHAR(45) NOT NULL,
  `department` INT NOT NULL,
  `position` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  INDEX `fk_employee_department_idx` (`department` ASC) VISIBLE,
  CONSTRAINT `fk_employee_department`
    FOREIGN KEY (`department`)
    REFERENCES `station`.`department` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`document_type`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`document_type` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `type` VARCHAR(45) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`issuing_authority`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`issuing_authority` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(200) NULL DEFAULT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`document`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`document` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `series` VARCHAR(200) NOT NULL,
  `number` VARCHAR(45) NOT NULL,
  `date_of_issue` DATE NOT NULL,
  `document_type` INT NOT NULL,
  `issuing_authority` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_document_document_type1_idx` (`document_type` ASC) VISIBLE,
  INDEX `fk_document_issuing_authority1_idx` (`issuing_authority` ASC) VISIBLE,
  CONSTRAINT `fk_document_document_type1`
    FOREIGN KEY (`document_type`)
    REFERENCES `station`.`document_type` (`id`)
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_document_issuing_authority1`
    FOREIGN KEY (`issuing_authority`)
    REFERENCES `station`.`issuing_authority` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`organization`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`organization` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`visitor`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`visitor` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `lastname` VARCHAR(45) NOT NULL,
  `document` INT NOT NULL,
  `place_of_work` INT NULL DEFAULT NULL,
  `position` VARCHAR(45) NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_visitor_document1_idx` (`document` ASC) VISIBLE,
  INDEX `fk_visitor_organization1_idx` (`place_of_work` ASC) VISIBLE,
  CONSTRAINT `fk_visitor_document1`
    FOREIGN KEY (`document`)
    REFERENCES `station`.`document` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_visitor_organization1`
    FOREIGN KEY (`place_of_work`)
    REFERENCES `station`.`organization` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`car_pass`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`car_pass` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `car_id` INT NOT NULL,
  `purpose_of_issuance_id` VARCHAR(1000) NOT NULL,
  `pass_issued` INT NOT NULL,
  `briefing` INT NOT NULL,
  `entry_allowed` INT NOT NULL,
  `start` DATETIME NOT NULL,
  `end` DATETIME NOT NULL,
  `trailer` INT NOT NULL,
  `visitor` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_car_pass_car1_idx` (`car_id` ASC) VISIBLE,
  INDEX `fk_car_pass_employee1_idx` (`pass_issued` ASC) VISIBLE,
  INDEX `fk_car_pass_employee2_idx` (`briefing` ASC) VISIBLE,
  INDEX `fk_car_pass_employee3_idx` (`entry_allowed` ASC) VISIBLE,
  INDEX `fk_car_pass_trailer1_idx` (`trailer` ASC) VISIBLE,
  INDEX `fk_car_pass_visitor1_idx` (`visitor` ASC) VISIBLE,
  CONSTRAINT `fk_car_pass_car1`
    FOREIGN KEY (`car_id`)
    REFERENCES `station`.`car` (`id`),
  CONSTRAINT `fk_car_pass_employee1`
    FOREIGN KEY (`pass_issued`)
    REFERENCES `station`.`employee` (`id`),
  CONSTRAINT `fk_car_pass_employee2`
    FOREIGN KEY (`briefing`)
    REFERENCES `station`.`employee` (`id`),
  CONSTRAINT `fk_car_pass_employee3`
    FOREIGN KEY (`entry_allowed`)
    REFERENCES `station`.`employee` (`id`),
  CONSTRAINT `fk_car_pass_trailer1`
    FOREIGN KEY (`trailer`)
    REFERENCES `df`.`trailer` (`id`),
  CONSTRAINT `fk_car_pass_visitor1`
    FOREIGN KEY (`visitor`)
    REFERENCES `station`.`visitor` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`station_facility`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`station_facility` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(200) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`driving_route`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`driving_route` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `start_of_route` INT NOT NULL,
  `end_ of_route` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_driving_route_station_facility1_idx` (`start_of_route` ASC) VISIBLE,
  INDEX `fk_driving_route_station_facility2_idx` (`end_ of_route` ASC) VISIBLE,
  CONSTRAINT `fk_driving_route_station_facility1`
    FOREIGN KEY (`start_of_route`)
    REFERENCES `station`.`station_facility` (`id`),
  CONSTRAINT `fk_driving_route_station_facility2`
    FOREIGN KEY (`end_ of_route`)
    REFERENCES `station`.`station_facility` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`operator`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`operator` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `login` VARCHAR(45) NOT NULL,
  `password` VARCHAR(45) NOT NULL,
  `employee_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_operator_employee1_idx` (`employee_id` ASC) VISIBLE,
  CONSTRAINT `fk_operator_employee1`
    FOREIGN KEY (`employee_id`)
    REFERENCES `station`.`employee` (`id`))
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`shooting_permission`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`shooting_permission` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `camera_type` VARCHAR(45) NULL DEFAULT NULL,
  `shooting_allowed` INT NOT NULL,
  `shooting_purpose` VARCHAR(1000) NOT NULL,
  `start` DATETIME NULL DEFAULT NULL,
  `end` DATETIME NULL DEFAULT NULL,
  `subject_of_shooting` INT NOT NULL,
  `visitor` INT NULL DEFAULT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_shooting_permission_employee1_idx` (`shooting_allowed` ASC) VISIBLE,
  INDEX `fk_shooting_permission_station_facility1_idx` (`subject_of_shooting` ASC) VISIBLE,
  INDEX `fk_shooting_permission_visitor1_idx` (`visitor` ASC) VISIBLE,
  CONSTRAINT `fk_shooting_permission_employee1`
    FOREIGN KEY (`shooting_allowed`)
    REFERENCES `station`.`employee` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_shooting_permission_station_facility1`
    FOREIGN KEY (`subject_of_shooting`)
    REFERENCES `station`.`station_facility` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_shooting_permission_visitor1`
    FOREIGN KEY (`visitor`)
    REFERENCES `station`.`visitor` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`single_pass`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`single_pass` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `pass_issued` DATETIME NOT NULL,
  `valid_until` DATETIME NOT NULL,
  `single_pass_issued` INT NULL DEFAULT NULL,
  `purpose_of_issuance` VARCHAR(1000) NULL DEFAULT NULL,
  `accompanying` INT NULL DEFAULT NULL,
  `visitor` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_single_pass_employee1_idx` (`single_pass_issued` ASC) VISIBLE,
  INDEX `fk_single_pass_employee2_idx` (`accompanying` ASC) VISIBLE,
  INDEX `fk_single_pass_visitor1_idx` (`visitor` ASC) VISIBLE,
  CONSTRAINT `fk_single_pass_employee1`
    FOREIGN KEY (`single_pass_issued`)
    REFERENCES `station`.`employee` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_single_pass_employee2`
    FOREIGN KEY (`accompanying`)
    REFERENCES `station`.`employee` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_single_pass_visitor1`
    FOREIGN KEY (`visitor`)
    REFERENCES `station`.`visitor` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`temporar_pass`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`temporar_pass` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `valid_with` DATE NOT NULL,
  `valid_until` DATE NOT NULL,
  `purpose_of_issuance` VARCHAR(500) NOT NULL,
  `pass_issued` INT NOT NULL,
  `visitor` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_temporar_ pass_employee1_idx` (`pass_issued` ASC) VISIBLE,
  INDEX `fk_temporar_ pass_visitor1_idx` (`visitor` ASC) VISIBLE,
  CONSTRAINT `fk_temporar_ pass_employee1`
    FOREIGN KEY (`pass_issued`)
    REFERENCES `station`.`employee` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_temporar_ pass_visitor1`
    FOREIGN KEY (`visitor`)
    REFERENCES `station`.`visitor` (`id`)
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


-- -----------------------------------------------------
-- Table `station`.`station_access`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `station`.`station_access` (
  `station_facility` INT NOT NULL,
  `access_ level` INT NOT NULL,
  `single_pass_id` INT NULL DEFAULT NULL,
  `temporar_ pass_id` INT NULL DEFAULT NULL,
  PRIMARY KEY (`station_facility`, `access_ level`),
  INDEX `fk_station_facility_has_access_ level_access_ level1_idx` (`access_ level` ASC) VISIBLE,
  INDEX `fk_station_facility_has_access_ level_station_facility1_idx` (`station_facility` ASC) VISIBLE,
  INDEX `fk_station_access_single_pass1_idx` (`single_pass_id` ASC) VISIBLE,
  INDEX `fk_station_access_temporar_ pass1_idx` (`temporar_ pass_id` ASC) VISIBLE,
  CONSTRAINT `fk_station_access_single_pass1`
    FOREIGN KEY (`single_pass_id`)
    REFERENCES `station`.`single_pass` (`id`),
  CONSTRAINT `fk_station_access_temporar_ pass1`
    FOREIGN KEY (`temporar_ pass_id`)
    REFERENCES `station`.`temporar_pass` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_station_facility_has_access_ level_access_ level1`
    FOREIGN KEY (`access_ level`)
    REFERENCES `station`.`access_ level` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT,
  CONSTRAINT `fk_station_facility_has_access_ level_station_facility1`
    FOREIGN KEY (`station_facility`)
    REFERENCES `station`.`station_facility` (`id`)
    ON DELETE RESTRICT
    ON UPDATE RESTRICT)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

";

        public static void DropDBStation()
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["mySqlConnection"].ConnectionString))
            {
                var command = new MySqlCommand();
                command.Connection = connection;
                connection.Open();
                command.CommandText = "DROP DATABASE IF EXISTS station";
                command.ExecuteNonQuery();
            }
        }

        public static void CreateDb()
        {
            using (var connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["mySqlConnection"].ConnectionString))
            {
                var command = new MySqlCommand();
                command.Connection = connection;
                connection.Open();
                command.CommandText = createSqlString;
                command.ExecuteNonQuery();
            }
        }


    }
}


