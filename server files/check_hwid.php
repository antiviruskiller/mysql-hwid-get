<?php
// Database connection details
$servername = "localhost";
$username = "username";
$password = "password";
$dbname = "dbname";

// Create connection
$conn = mysqli_connect($servername, $username, $password, $dbname);

// Check connection
if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
}

// Check if the HWID is banned
$hwid = $_GET['hwid'];
$sql = "SELECT * FROM hwid_status WHERE hwid = '$hwid' AND status = 1";
$result = mysqli_query($conn, $sql);

if (mysqli_num_rows($result) > 0) {
    echo "banned";
} else {
    echo "not_banned";
}

// Close the database connection
mysqli_close($conn);
?>
