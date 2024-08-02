module Rommulbad.Model.Validation.Guardian

open System
open System.Text.RegularExpressions

module Guardian =
    let private isIdValid (id: string) = Regex.IsMatch(id, @"^\d{3}-[A-Z]{4}$")
    
    let private isNameValid (name: string) = Regex.IsMatch(name, @"^[A-Za-z]+( [A-Za-z]+)*$")
    
