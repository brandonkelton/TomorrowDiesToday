using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using TomorrowDiesToday.Models;
using TomorrowDiesToday.Models.Enums;

namespace TomorrowDiesToday.Services.Game
{
    public class SquadService : ISquadService
    {
        #region Properties

        // Observables
        public IObservable<string> ErrorMessage => _errorMessage;
        public IObservable<List<SquadModel>> SelectedSquadsUpdate => _selectedSquadsUpdate;
        public IObservable<SquadStats> SelectedSquadStatsUpdate => _selectedSquadStatsUpdate;
        public IObservable<SquadModel> SquadUpdate => _squadUpdate;

        private readonly ReplaySubject<string> _errorMessage = new ReplaySubject<string>(1);
        private readonly ReplaySubject<List<SquadModel>> _selectedSquadsUpdate = new ReplaySubject<List<SquadModel>>(1);
        private readonly ReplaySubject<SquadStats> _selectedSquadStatsUpdate = new ReplaySubject<SquadStats>(1);
        private readonly ReplaySubject<SquadModel> _squadUpdate = new ReplaySubject<SquadModel>(1);

        // Required Service(s)
        private IGameService _gameService;

        List<SquadModel> _selectedSquads => _gameService.Game.Players.SelectMany(player => player.Squads.Where(squad => squad.IsSelected)).ToList();

        #endregion

        #region Public Methods

        public void CalculateSquadStats(SquadModel squadModel)
        {
            //Faceless Henchman
            squadModel.Stats.Combat.SetValue(squadModel.Armaments.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Combat.Value));
            squadModel.Stats.Stealth.SetValue(squadModel.Armaments.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Stealth.Value));
            squadModel.Stats.Cunning.SetValue(squadModel.Armaments.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Cunning.Value));
            squadModel.Stats.Diplomacy.SetValue(squadModel.Armaments.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Diplomacy.Value));

            //Abilities
            squadModel.Stats.Combat.SetValue(squadModel.Abilities.Where(a =>a.Count > 0).Sum(a => a.Count * a.Stats.Combat.Value));
            squadModel.Stats.Stealth.SetValue(squadModel.Abilities.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Stealth.Value));
            squadModel.Stats.Cunning.SetValue(squadModel.Abilities.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Cunning.Value));
            squadModel.Stats.Diplomacy.SetValue(squadModel.Abilities.Where(a => a.Count > 0).Sum(a => a.Count * a.Stats.Diplomacy.Value));

            //Items
            squadModel.Stats.Combat.SetValue(squadModel.Items.Where(a =>a.Count > 0).Sum(a => a.Count * a.Stats.Combat.Value));
            squadModel.Stats.Combat.SetValue(squadModel.Items.Where(a =>a.Count > 0).Sum(a => a.Count * a.Stats.Combat.Value));
            squadModel.Stats.Combat.SetValue(squadModel.Items.Where(a =>a.Count > 0).Sum(a => a.Count * a.Stats.Combat.Value));
            squadModel.Stats.Combat.SetValue(squadModel.Items.Where(a =>a.Count > 0).Sum(a => a.Count * a.Stats.Combat.Value));


            if (squadModel.IsSelected)
            {
                var selectedSquads = _gameService.Game.Players.SelectMany(player => player.Squads.Where(squad => squad.IsSelected)).ToList();
                _selectedSquadsUpdate.OnNext(selectedSquads);
            }

            _squadUpdate.OnNext(squadModel);

            if (squadModel.IsSelected)
            {
                _selectedSquadsUpdate.OnNext(_selectedSquads);
                SumSelectedSquadStats();
            }
        }

        public void DecrementAbilityCount(ArmamentType abilityType, SquadModel squadModel)
        {
            var ability = squadModel.Armaments.Where(a => a.ArmamentType == abilityType).FirstOrDefault();
            if (ability != null)
            {
                if (ability.Count - 1 >= 0)
                {
                    ability.SetCount(ability.Count - 1);
                    CalculateSquadStats(squadModel);
                }
                else
                {
                    _errorMessage.OnNext(ErrorType.InvalidAbilityCount.ToDescription());
                }
            }
            else
            {
                _errorMessage.OnNext(ErrorType.InvalidAbilityType.ToDescription());
            }
        }

        public void DecrementArmamentCount(ArmamentType armamentType, SquadModel squadModel)
        {
            var armament = squadModel.Armaments.Where(a => a.ArmamentType == armamentType).FirstOrDefault();
            if (armament != null)
            {
                if (armament.Count - 1 >= 0)
                {
                    armament.SetCount(armament.Count - 1);
                    CalculateSquadStats(squadModel);
                }
                else
                {
                    if (armament.ArmamentType == _gameService.Game.PlayerType)
                    {
                        _errorMessage.OnNext(ErrorType.InvalidNamedHenchmanCount.ToDescription());
                    }
                    else
                    {
                        _errorMessage.OnNext(ErrorType.InvalidArmamentCount.ToDescription());
                    }
                }
            }
            else
            {
                _errorMessage.OnNext(ErrorType.InvalidArmamentType.ToDescription());
            }
        }

        public void DecrementItemCount(ArmamentType itemType, SquadModel squadModel)
        {
            var item = squadModel.Armaments.Where(a => a.ArmamentType == itemType).FirstOrDefault();
            if (item != null)
            {
                if (item.Count - 1 >= 0)
                {
                    item.SetCount(item.Count - 1);
                    CalculateSquadStats(squadModel);
                }
                else
                {
                    _errorMessage.OnNext(ErrorType.InvalidAbilityCount.ToDescription());
                }
            }
            else
            {
                _errorMessage.OnNext(ErrorType.InvalidItemType.ToDescription());
            }
        }

        public void IncrementAbilityCount(ArmamentType abilityType, SquadModel squadModel)
        {
            ArmamentType playerArmamentType = ((ArmamentType)int.Parse(_gameService.Game.PlayerId));
            if (playerArmamentType == ArmamentType.UgoDottore)
            {
                var ability = squadModel.Armaments.Where(a => a.ArmamentType == abilityType).FirstOrDefault();

                if (ability != null)
                {
                    var squads = _gameService.Game.Players.Where(p => p.PlayerId == squadModel.PlayerId).FirstOrDefault().Squads;
                    var abilityArmament = squads.Select(s => s.Abilities.Where(a => a.Count > 0).FirstOrDefault()).FirstOrDefault();

                    if (abilityArmament != null)
                    {
                        if (!ability.Equals(abilityArmament))
                        {
                            abilityArmament.SetCount(0);
                            ability.SetCount(1);
                            CalculateSquadStats(squadModel);
                        }
                        else
                        {
                            _errorMessage.OnNext(ErrorType.InvalidAbilityCount.ToDescription());
                        }
                    }
                    else
                    {
                        ability.SetCount(1);
                        CalculateSquadStats(squadModel);
                    }
                }
            }
            else
            {
                _errorMessage.OnNext(ErrorType.InvalidNamedHenchmanType.ToDescription());
            }
        }

        public void IncrementArmamentCount(ArmamentType armamentType, SquadModel squadModel)
        {
            var armament = squadModel.Armaments.Where(a => a.ArmamentType == armamentType).FirstOrDefault();
            var validTotalArmamentCount = 6;
            var totalArmamentCount = squadModel.Armaments.Sum(a => a.Count);
            if (armament != null)
            {
                if (totalArmamentCount + 1 <= validTotalArmamentCount)
                {
                    var playerArmamentType = _gameService.Game.PlayerType;

                    if (armament.ArmamentType == _gameService.Game.PlayerType)
                    {
                        var playerId = _gameService.Game.PlayerId;
                        var squads = _gameService.Game.Players.Where(p => p.PlayerId == playerId).FirstOrDefault().Squads;
                        var namedHenchmanArmament = squads.Select(s => s.Armaments.Where(a => a.ArmamentType == playerArmamentType && a.Count > 0).FirstOrDefault()).FirstOrDefault();

                        if (namedHenchmanArmament != null)
                        {
                            if (!armament.Equals(namedHenchmanArmament))
                            {
                                namedHenchmanArmament.SetCount(0);
                                armament.SetCount(1);
                                CalculateSquadStats(squadModel);
                            }
                            else
                            {
                                _errorMessage.OnNext(ErrorType.InvalidNamedHenchmanCount.ToDescription());
                            }
                        }
                        else
                        {
                            armament.SetCount(1);
                            CalculateSquadStats(squadModel);
                        }
                    }
                    armament.SetCount(armament.Count + 1);
                    CalculateSquadStats(squadModel);
                }
                else
                {
                    _errorMessage.OnNext(ErrorType.InvalidSquadSize.ToDescription());
                }
            }
            else
            {
                _errorMessage.OnNext(ErrorType.InvalidArmamentType.ToDescription());
            }
        }

        public void IncrementItemCount(ArmamentType itemType, SquadModel squadModel)
        {
            var item = squadModel.Armaments.Where(a => a.ArmamentType == itemType).FirstOrDefault();

            if (item != null)
            {
                var squads = _gameService.Game.Players.Where(p => p.PlayerId == squadModel.PlayerId).FirstOrDefault().Squads;
                var itemArmament = squads.Select(s => s.Items.Where(a => a.ArmamentType == itemType && a.Count > 0).FirstOrDefault()).FirstOrDefault();

                if (itemArmament != null)
                {
                    if (!item.Equals(itemArmament))
                    {
                        item.SetCount(1);
                        CalculateSquadStats(squadModel);
                    }
                    else
                    {
                        _errorMessage.OnNext(ErrorType.InvalidAbilityCount.ToDescription());
                    }
                }
                else
                {
                    item.SetCount(1);
                    CalculateSquadStats(squadModel);
                }
            }
        }

        public void ToggleSelected(SquadModel squadModel)
        {
            squadModel.IsSelected = !squadModel.IsSelected;
            _selectedSquadsUpdate.OnNext(_selectedSquads);
            SumSelectedSquadStats();
            _squadUpdate.OnNext(squadModel);
        }

        #endregion

        #region Helper Methods

        private void SumSelectedSquadStats()
        {
            SquadStats stats = _gameService.Game.SelectedSquadStats;

            stats.Combat.SetValue(_selectedSquads.Sum(a => a.Stats.Combat.Value));
            stats.Stealth.SetValue(_selectedSquads.Sum(a => a.Stats.Stealth.Value));
            stats.Cunning.SetValue(_selectedSquads.Sum(a => a.Stats.Cunning.Value));
            stats.Diplomacy.SetValue(_selectedSquads.Sum(a => a.Stats.Diplomacy.Value));

            _selectedSquadStatsUpdate.OnNext(stats);
        }

        #endregion
    }
}
