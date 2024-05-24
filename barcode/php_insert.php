<?php
// Database configuration
$servername = "localhost";
$username = "root";
$password = "";
$dbname = "qrcodereader";

// Create connection
$conn = new mysqli($servername, $username, $password, $dbname);

// Check connection
if ($conn->connect_error) {
    $response = array("success" => false, "message" => "Connection failed: " . $conn->connect_error);
    echo json_encode($response);
    exit();
}

// Check if format and value data are received
if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['format']) && isset($_POST['value'])) {
    $format = $_POST['format'];
    $value = $_POST['value'];

    // Prepare an insert statement
    $stmt = $conn->prepare("INSERT INTO barcodes (format, value) VALUES (?, ?)");
    $stmt->bind_param("ss", $format, $value);

    // Execute the statement
    if ($stmt->execute()) {
        $response = array("success" => true, "message" => "New record created successfully");
    } else {
        $response = array("success" => false, "message" => "Error: " . $stmt->error);
    }

    // Close the statement
    $stmt->close();
} else {
    $response = array("success" => false, "message" => "Invalid request");
}

// Close the connection
$conn->close();

// Return the JSON response
echo json_encode($response);
?>
