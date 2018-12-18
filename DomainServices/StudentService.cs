﻿using Common;
using Common.Commanding;
using Domain;
using Domain.StudentDomain;
using Domain.StudentDomain.Commands;

namespace DomainServices
{
    public sealed class StudentService
    {
        private readonly ICommandBus<Student>  _commandBus;

        public StudentService(ICommandBus<Student> commandBus)
        {
            _commandBus = commandBus;
        }
        
        public void AddNewStudentFrom(
            EmailAddress emailAddress,
            Name name,
            Maybe<City> maybeCity,
            bool isEmployed) => _commandBus.Enqueue(
            new AddNewStudent(
                emailAddress,
                name,
                maybeCity,
                isEmployed));
    }
}