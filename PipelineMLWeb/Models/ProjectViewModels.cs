﻿using PipelineMLCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PipelineMLWeb.Models
{
    // Shows description on an instantiable pipeline part class for a user
    // to select for their pipeline. once selected, should be added to 
    // the pipeline definition
    public class SearchableClassViewModel
    {
        public string FriendlyName { get; set; }

        public string Description { get; set; }

        public Type ClassType { get; set; }

    }


    // provides the UI with lists of available pipeline part classes to choose from
    public class PipelinePartListViewModel
    {

        private static string _datasetGenerator;
        public string DatasetGenerator { get; set; }


        private static string _dataTranforms;
        public string DataTransform { get; set; }

        private static string _machineLearning;
        public string MachineLearning { get; set; }


        private static string _evaluator;
        public string Evaluator { get; set; }


        private static Dictionary<string, List<SearchableClassViewModel>> _pipelineParts = null;
        public Dictionary<string, List<SearchableClassViewModel>> PipelineParts { get { return _pipelineParts; } }


        public PipelinePartListViewModel()
        {
            if (_pipelineParts == null)
            {
                _datasetGenerator = typeof(IDatasetGenerator).ToString();
                _dataTranforms = typeof(IDataTransform).ToString();
                _machineLearning = typeof(IMachineLearningProcess).ToString();
                _evaluator = typeof(IEvaluator).ToString();

                _pipelineParts = new Dictionary<string, List<SearchableClassViewModel>>();
                _pipelineParts.Add(_datasetGenerator, GetPartsList(typeof(IDatasetGenerator)));
                _pipelineParts.Add(_dataTranforms, GetPartsList(typeof(IDataTransform)));
                _pipelineParts.Add(_machineLearning, GetPartsList(typeof(IMachineLearningProcess)));
                _pipelineParts.Add(_evaluator, GetPartsList(typeof(IEvaluator)));
            }
        }

        private List<SearchableClassViewModel> GetPartsList(Type partInterface)
        {
            var TypeList = SearchClasses.SearchForClassesThatImplementInterface(partInterface);
            List<SearchableClassViewModel> partsList = new List<SearchableClassViewModel>();
            foreach (var item in TypeList)
            {
                ISearchableClass iSearchable = Activator.CreateInstance(item) as ISearchableClass;
                partsList.Add(new SearchableClassViewModel() { FriendlyName = iSearchable.FriendlyName, Description = iSearchable.Description, ClassType = item });
            }
            return partsList;
        }
    }


    // Defines the shell of the pieces of a pipeline
    // to be used to get the details of the part when necessary
    public class PipelinePartViewModel
    {
        public Guid Id { get; set; }

        [Required, MaxLength(40)]
        public string Name { get; set; }

        public string ClassName { get; set; }

        public int x { get; set; }

        public int y { get; set; }

    }



    /// <summary>
    /// Container and View Model for the Project definition Editor
    /// </summary>
    public class ProjectViewModel
    {

        public Guid Id { get; set; }

        [Required, MaxLength(40)]
        public string Name { get; set; }

        public string Description { get; set; }

        public PipelinePartViewModel DataGeneratorPart { get; set; }

        public List<PipelinePartViewModel> PreProcessParts { get; set; }

        public List<PipelinePartViewModel> MLParts { get; set; }

        public List<PipelinePartViewModel> PostProcessParts { get; set; }

        public List<PipelinePartViewModel> EvalutorParts { get; set; }



        public ProjectViewModel()
        {
            Init();
        }


        private void Init()
        {
            PreProcessParts = new List<PipelinePartViewModel>();
            MLParts = new List<PipelinePartViewModel>();
            PostProcessParts = new List<PipelinePartViewModel>();
            EvalutorParts = new List<PipelinePartViewModel>();
        }


        public ProjectViewModel(PipelineProject proj)
        {
            Init();
            Name = proj.Name;
            Id = proj.Id;
            Description = proj.Description;
        }


        public void SetDefinition(PipelineDefinition def)
        {
            var inst = def.CreateInstance();
            DataGeneratorPart = new PipelinePartViewModel();
            DataGeneratorPart.ClassName = inst.DatasetGenerator.GetType().Name;
            DataGeneratorPart.Name = inst.DatasetGenerator.Name;
            DataGeneratorPart.Id = inst.DatasetGenerator.Id;

            foreach (var item in inst.PreprocessDataTransforms)
            {
                var part = new PipelinePartViewModel();
                part.Id = item.Id;
                part.Name = item.Name;
                part.ClassName = item.GetType().Name;
            }
        }


    }
}