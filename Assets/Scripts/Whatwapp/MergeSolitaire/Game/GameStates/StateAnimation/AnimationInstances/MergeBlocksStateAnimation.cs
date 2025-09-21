using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Whatwapp.Core.Audio;
using Whatwapp.Core.Extensions;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MergeBlocksStateAnimation : IStateAnimation
    {
        private readonly GameController _gameController;
        private readonly FoundationsController _foundationsController;
        private readonly BlockFactory _blockFactory;
        private readonly List<List<Cell>> _mergeableGroups;

        private Sequence _sequence;
        public MergeBlocksStateAnimation(GameController gameController, FoundationsController foundationsController, BlockFactory blockFactory, List<List<Cell>> mergeableGroups)
        {
            _gameController = gameController;
            _foundationsController = foundationsController;
            _blockFactory = blockFactory;
            _mergeableGroups = mergeableGroups;
        }
        
        public void Play(Action onComplete)
        {
             _sequence = DOTween.Sequence();
            foreach (var group in _mergeableGroups)
            {
                var seedHash = new HashSet<BlockSeed>();
                var firstCell = group[0];
                var value = firstCell.Block.Value;
                var seed = firstCell.Block.Seed;
                seedHash.Add(seed);
                var groupSequence = DOTween.Sequence();
                var tremorSequence = DOTween.Sequence();
                foreach (var cell in group)
                {
                    seedHash.Add(cell.Block.Seed);
                    tremorSequence.Join(cell.Block.Visual.PlayTremorAnimation());
                }

                Debug.Log("Seed hash count: " + seedHash.Count);

                groupSequence.Append(tremorSequence);
                for (var i = group.Count - 1; i > 0; i--)
                {
                    var cell = group[i];
                    var block = cell.Block;
                    var targetCell = group[i - 1];
                    var targetPos = targetCell.transform.position;
                    var blockSequence = block.Visual.PlayMergeAnimation(targetPos);
                    blockSequence.OnComplete(() =>
                    {
                        SFXManager.Instance.PlayOneShot(Consts.SFX_MergeBlocks);
                        targetCell.Block = null;
                        _gameController.Score += _mergeableGroups.Count * group.Count;
                        block.Remove();
                    });
                    groupSequence.Join(blockSequence);
                }

                var finalSequence = DOTween.Sequence();
                finalSequence.Append(firstCell.Block.Visual.PlayScaleDownAnimation())
                    .OnComplete(() =>
                    {
                        firstCell.Block.Remove();
                        firstCell.Block = null;
                    });
                groupSequence.Join(finalSequence);
                groupSequence.OnComplete(() =>
                {
                    var nextValue = value.Next(true);
                    var newBlock = _blockFactory.Create(nextValue, seed);
                    firstCell.Block = newBlock;
                    newBlock.Visual.PlayScaleUpAnimation();

                    foreach (var seedInGroup in seedHash)
                    {
                        Debug.Log("Seed in group: " + seedInGroup);
                        var info = new BlockToFoundationInfo(seedInGroup, value, firstCell.Position);
                        if (_foundationsController.TryAndAttach(info))
                        {
                            SFXManager.Instance.PlayOneShot(Consts.GetFoundationSFX(seedInGroup));
                            _gameController.Score += Consts.FOUNDATION_POINTS;
                        }
                    }
                });

                _sequence.Append(groupSequence);
            }

            _sequence.OnComplete(onComplete.Invoke);
            _sequence.Play();
        }

        public void Kill(bool complete)
        {
            _sequence.Kill(complete);
        }
    }
}