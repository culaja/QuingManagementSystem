﻿using System;
using System.Runtime.CompilerServices;
using Common;
using Domain.StudentDomain.Events;
using static System.Guid;

namespace Domain.StudentDomain
{
    public class Student : AggregateRoot
    {
        public Name Name { get; }
        
        public EmailAddress EmailAddress { get; }
        public Maybe<City> MaybeCity { get; private set; } = Maybe<City>.None;
        public bool IsHired { get; private set; } = false;
        
        public Student(
            Guid id,
            ulong version,
            Name name,
            EmailAddress emailAddress,
            Maybe<City> maybeCity,
            bool isHired) 
            : base(id, version)
        {
            Name = name;
            EmailAddress = emailAddress;
            MaybeCity = maybeCity;
            IsHired = isHired;
        }

        public static Student NewStudentFrom(
            Guid id,
            Name name,
            EmailAddress emailAddress,
            Maybe<City> maybeCity,
            bool isHired)
        {
            var student = new Student(
                id,
                0,
                name,
                emailAddress,
                maybeCity,
                isHired);
            student.Add(new StudentCreated(
                student.Id,
                typeof(Student),
                student.Version,
                student.Name,
                student.EmailAddress,
                student.MaybeCity,
                student.IsHired));
            return student;
        }

        public Student MoveTo(City city)
        {
            MaybeCity = city;
            Add(new StudentMoved(Id, IncrementedVersion(), city));
            return this;
        }

        public Student Apply(StudentMoved studentMoved)
        {
            MaybeCity = studentMoved.City;
            return this;
        }

        public Student GetAJob()
        {
            IsHired = true;
            Add(new StudentHired(Id, IncrementedVersion()));
            return this;
        }

        public Student Apply(StudentHired studentHired)
        {
            IsHired = true;
            return this;
        }
    }
}