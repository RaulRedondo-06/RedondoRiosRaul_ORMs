using UnityEngine;
using SQLite4Unity3d;
using System;
using System.Collections.Generic;
using System.Linq;

public class SQLiteDB : MonoBehaviour
{
    private SQLiteConnection db;
    private string dbPath;

    private void Start()
    {
        dbPath = Application.persistentDataPath + "/Users.sqlite";

        try
        {
            db = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            db.CreateTable<User>();
            Debug.Log("Base de datos creada/abierta en: " + dbPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error al abrir la base de datos: " + e.Message);
        }
    }

    private void OnDestroy()
    {
        if (db != null)
        {
            db.Close();
            db = null;
        }
    }

    public string Register(string user, string pass)
    {
        if (db == null)
            return "Error amb la base de dades";

        user = user.Trim();
        pass = pass.Trim();

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            return "Omple tots els camps";

        if (pass.Length < 8)
            return "Password mínim 8 caràcters";

        try
        {
            User existingUser = db.Table<User>().FirstOrDefault(u => u.Username == user);

            if (existingUser != null)
                return "Usuari ja existeix";

            User newUser = new User
            {
                Username = user,
                Password = pass
            };

            db.Insert(newUser);
            return "Registre correcte";
        }
        catch (Exception e)
        {
            Debug.LogError("Error al registrar: " + e.Message);
            return "Error al registrar l'usuari";
        }
    }

    public string Login(string user, string pass)
    {
        if (db == null)
            return "Error amb la base de dades";

        user = user.Trim();
        pass = pass.Trim();

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            return "Omple tots els camps";

        try
        {
            User foundUser = db.Table<User>().FirstOrDefault(u => u.Username == user);

            if (foundUser == null)
                return "Usuari no trobat";

            if (foundUser.Password != pass)
                return "Contrasenya incorrecta";

            PlayerPrefs.SetString("logged_user", foundUser.Username);
            PlayerPrefs.Save();

            return "OK";
        }
        catch (Exception e)
        {
            Debug.LogError("Error al login: " + e.Message);
            return "Error al fer login";
        }
    }

    public string GetUsersList()
    {
        if (db == null)
            return "Error amb la base de dades";

        try
        {
            List<User> users = db.Table<User>().OrderBy(u => u.UserID).ToList();

            if (users.Count == 0)
                return "No hi ha usuaris registrats";

            string result = "Usuaris registrats:\n\n";

            foreach (User user in users)
            {
                result += "ID: " + user.UserID + " | " + user.Username + "\n";
            }

            return result;
        }
        catch (Exception e)
        {
            Debug.LogError("Error obtenint usuaris: " + e.Message);
            return "Error en obtenir els usuaris";
        }
    }

    public string DeleteUser(string username)
    {
        if (db == null)
            return "Error amb la base de dades";

        username = username.Trim();

        if (string.IsNullOrEmpty(username))
            return "Introdueix un usuari";

        try
        {
            User userToDelete = db.Table<User>().FirstOrDefault(u => u.Username == username);

            if (userToDelete == null)
                return "Usuari no trobat";

            db.Delete(userToDelete);
            return "Usuari esborrat correctament";
        }
        catch (Exception e)
        {
            Debug.LogError("Error esborrant usuari: " + e.Message);
            return "Error en esborrar l'usuari";
        }
    }

    public string DeleteAllUsers()
    {
        if (db == null)
            return "Error amb la base de dades";

        try
        {
            List<User> users = db.Table<User>().ToList();

            if (users.Count == 0)
                return "No hi ha usuaris per esborrar";

            foreach (User user in users)
            {
                db.Delete(user);
            }

            return "Tots els usuaris han estat esborrats";
        }
        catch (Exception e)
        {
            Debug.LogError("Error esborrant usuaris: " + e.Message);
            return "Error en esborrar els usuaris";
        }
    }
}