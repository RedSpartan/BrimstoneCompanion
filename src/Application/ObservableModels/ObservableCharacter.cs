﻿using CommunityToolkit.Mvvm.ComponentModel;
using RedSpartan.BrimstoneCompanion.Domain.Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace RedSpartan.BrimstoneCompanion.AppLayer.ObservableModels
{
    public partial class ObservableCharacter : ObservableModel<Character>
    {
        [ObservableProperty]
        private int _currentWeight;

        private bool _initialised = false;

        public ObservableCharacter(string name, string role)
            : this(new Character { Name = name, Class = role })
        { }

        private ObservableCharacter(Character character) : base(character)
        {
            InitialiseFeatures();

            InitialiseAttributes();

            InitialiseNotes();

            InitialiseKeywords();

            InitialiseTokens();
        }

        #region Properties

        public string Id
        {
            get => Model.Id;
            set => SetProperty(Model.Id, value, Model, (model, _value) => model.Id = _value);
        }

        public string Name
        {
            get => Model.Name;
            set => SetProperty(Model.Name, value, Model, (model, _value) => model.Name = _value);
        }

        public string Class
        {
            get => Model.Class;
            set => SetProperty(Model.Class, value, Model, (model, _value) => model.Class = _value);
        }

        public byte Level
        {
            get => Model.Level;
            set => SetProperty(Model.Level, value, Model, (model, _value) => model.Level = _value);
        }

        public bool Initialised => _initialised;

        //TODO: set the weight
        //public int CurrentWeight => GetAttribute(AttributeNames.HEAVY).Value + Features.Sum(x => x.Weight);

        #endregion Properties

        #region Collections

        public ObservableCollection<ObservableKeyword> Keywords { get; } = new ObservableCollection<ObservableKeyword>();

        public ObservableCollection<ObservableToken> Tokens { get; } = new ObservableCollection<ObservableToken>();

        public IDictionary<string, ObservableAttribute> Attributes { get; } = new Dictionary<string, ObservableAttribute>();

        public ObservableCollection<ObservableFeature> Features { get; set; } = new ObservableCollection<ObservableFeature>();

        public IList<ObservableKeyword> ConcatKeywords => Keywords.Concat(Features.SelectMany(x => x.Keywords)).ToList();

        public ObservableCollection<ObservableNote> Notes { get; set; } = new ObservableCollection<ObservableNote>();

        #endregion Collections

        #region Methods

        public static ObservableCharacter New(Character character) => new(character);

        public static ObservableCharacter New() => new(new Character());

        #endregion Methods

        #region Model Construction

        private void InitialiseAttributes()
        {
            if (Model.Attributes is null)
            {
                Model.Attributes = new Dictionary<string, AttributeValue>();
            }

            foreach (var attributeValue in Model.Attributes)
            {
                var attribute = ObservableAttribute.New(attributeValue.Key, attributeValue.Value);
                attribute.SetCurrentValues(Features);
                Attributes.Add(attributeValue.Key, attribute);
            }
        }

        private void InitialiseFeatures()
        {
            if (Model.Features is null)
            {
                Model.Features = new List<Feature>();
            }

            foreach (var feature in Model.Features)
            {
                Features.Add(ObservableFeature.New(feature));
            }

            Features.CollectionChanged += Features_CollectionChanged;
        }

        private void InitialiseNotes()
        {
            if (Model.Notes is null)
            {
                Model.Notes = new List<Note>();
            }

            foreach (var note in Model.Notes)
            {
                Notes.Add(ObservableNote.New(note));
            }

            Notes.CollectionChanged += Notes_CollectionChanged;
        }

        private void InitialiseKeywords()
        {
            if (Model.Keywords is null)
            {
                Model.Keywords = new List<Keyword>();
            }

            foreach (var keyword in Model.Keywords)
            {
                Keywords.Add(ObservableKeyword.New(keyword));
            }

            Keywords.CollectionChanged += Keywords_CollectionChanged;
        }

        private void InitialiseTokens()
        {
            if (Model.Tokens is null)
            {
                Model.Tokens = new List<Token>();
            }

            foreach (var token in Model.Tokens)
            {
                Tokens.Add(ObservableToken.New(token));
            }

            Tokens.CollectionChanged += Tokens_CollectionChanged;
        }

        private void Features_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            SubscribeToCollection<Feature, ObservableModel<Feature>>(args, Model.Features);
        }

        private void Notes_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            SubscribeToCollection<Note, ObservableModel<Note>>(args, Model.Notes);
        }

        private void Keywords_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            SubscribeToCollection<Keyword, ObservableModel<Keyword>>(args, Model.Keywords);
            OnPropertyChanged(nameof(ConcatKeywords));
        }

        private void Tokens_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
        {
            SubscribeToCollection<Token, ObservableModel<Token>>(args, Model.Tokens);
        }

        private static void SubscribeToCollection<T, TModel>(NotifyCollectionChangedEventArgs args, IList<T> list)
            where TModel : ObservableModel<T>
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (args.NewItems == null)
                    {
                        return;
                    }
                    foreach (var item in args.NewItems)
                    {
                        if (item is TModel model)
                        {
                            list.Add(model.GetModel());
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (args.OldItems == null)
                    {
                        return;
                    }
                    foreach (var item in args.OldItems)
                    {
                        if (item is TModel model)
                        {
                            list.Remove(model.GetModel());
                        }
                    }
                    break;
            }
        }

        #endregion Model Construction
    }
}