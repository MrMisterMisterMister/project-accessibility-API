<p align="center"><a href="https://clodsire.nl" target="_blank">
    <picture>
        <source media="(prefers-color-scheme: dark)" srcset="https://github.com/MrMisterMisterMister/project-accessibility/blob/main/ClientApp/src/assets/img/brand/logo_white_text_dark.png">
        <img src="https://github.com/MrMisterMisterMister/project-accessibility/blob/main/ClientApp/src/assets/img/brand/logo_black_text_light.png" width="450px;">
    </picture>
</a></p>

[1]: https://clodsire.nl

<p/>

# Setting Up Test Database for MySQl

## Installation Steps for XAMP (Can also be done with MySQL Workbench but too lazy to add the steps)

1. **Clone the Repository:**
   Clone this repository to your local machine.

2. **Install XAMPP:**
   Download and install [XAMPP](https://www.apachefriends.org/download.html) on your system.

3. **Start XAMPP Services:**

- Launch XAMPP.
- Start both Apache and MySQL services from the XAMPP Control Panel.

4. **Access MySQL Admin Panel:**

- Open a browser.
- Go to `http://localhost/phpmyadmin/` or access the MySQL admin panel via XAMPP.

5. **Run the Program:**

- Navigate to the project directory.
- Run the application using either `dotnet run` or `dotnet watch`, make sure to cd in `./API` before running.
- Entity Framework Core will create the test database if it hasn't been created yet.

## Configuration (If Necessary)

- Navigate to the `appsettings.json` file in the project root.
- Update the `ConnectionStrings` section with your MySQL database details.

## Troubleshooting

If encountering migration issues, verify the correctness of the connection string.
