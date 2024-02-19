<?php
// Database connection
$servername = "localhost";
$username = "username";
$password = "password";
$dbname = "dbname";

// Get HWID from request
$hwid = $_POST['hwid'];

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
  die("Connection failed: " . $conn->connect_error);
}

// Prepare and execute SQL query
$sql = "INSERT INTO hwid_status (hwid) VALUES ('$hwid')";
if ($conn->query($sql) === TRUE) {
  echo "HWID inserted successfully";
} else {
  echo "Error inserting HWID: " . $conn->error;
}

$conn->close();
?>
