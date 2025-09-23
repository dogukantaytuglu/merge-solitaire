using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Whatwapp.Core.Audio;
using Whatwapp.Core.Extensions;
using Whatwapp.MergeSolitaire.Game.Events;

namespace Whatwapp.MergeSolitaire.Game.GameStates
{
    public class MergeBlocksStateAnimation : IStateAnimation
    {
        public bool IsAnimationActive { get; private set; }

        private readonly GameController _gameController;
        private readonly FoundationsController _foundationsController;
        private readonly BlockFactory _blockFactory;
        private readonly MergeBlocksState _mergeBlocksState;

        private Sequence _sequence;
        public MergeBlocksStateAnimation(GameController gameController, FoundationsController foundationsController, BlockFactory blockFactory, MergeBlocksState mergeBlocksState)
        {
            _gameController = gameController;
            _foundationsController = foundationsController;
            _blockFactory = blockFactory;
            _mergeBlocksState = mergeBlocksState;
        }
        
        public void Play()
        {
            IsAnimationActive = true;
             _sequence = DOTween.Sequence();
             var mergeableGroups = MergeBlocksState.MergeableGroupsBuffer;
             
            foreach (var group in mergeableGroups)
            {
                var seedHash = new HashSet<BlockSeed>();
                var firstCell = group[0];
                if (firstCell.Block is not MergeBlock firstBlock) continue;
                var value = firstBlock.Value;
                var seed = firstBlock.Seed;
                seedHash.Add(seed);
                var groupSequence = DOTween.Sequence();
                var tremorSequence = DOTween.Sequence();
                foreach (var cell in group)
                {
                    if (cell.Block is not MergeBlock mergeBlock) continue;
                    seedHash.Add(mergeBlock.Seed);
                    tremorSequence.Join(mergeBlock.PlayTremorAnimation());
                }
            
                Debug.Log("Seed hash count: " + seedHash.Count);
            
                groupSequence.Append(tremorSequence);
                for (var i = group.Count - 1; i > 0; i--)
                {
                    var cell = group[i];
                    if (cell.Block is not MergeBlock mergeBlock) continue;
                    var block = cell.Block;
                    var targetCell = group[i - 1];
                    var targetPos = targetCell.transform.position;
                    var blockSequence = mergeBlock.PlayMergeAnimation(targetPos);
                    blockSequence.OnComplete(() =>
                    {
                        SFXManager.Instance.PlayOneShot(Consts.SFX_MergeBlocks);
                        targetCell.Block = null;
                        EventBus<ScoreGained>.Raise(new ScoreGained(mergeableGroups.Count * group.Count, targetCell.Position));
                        block.Remove();
                    });
                    groupSequence.Join(blockSequence);
                }
            
                var finalSequence = DOTween.Sequence();
                finalSequence.Append(firstBlock.PlayScaleDownAnimation())
                    .OnComplete(() =>
                    {
                        firstBlock.Remove();
                        firstBlock = null;
                    });
                groupSequence.Join(finalSequence);
                groupSequence.OnComplete(() =>
                {
                    var nextValue = value.Next(true);
                    var newBlock = _blockFactory.CreateMergeBlock(nextValue, seed);
                    firstCell.Block = newBlock;
                    newBlock.transform.position = firstCell.Position;
                    newBlock.PlayMergeScaleUpAnimation();
            
                    foreach (var seedInGroup in seedHash)
                    {
                        Debug.Log("Seed in group: " + seedInGroup);
                        var info = new BlockToFoundationInfo(seedInGroup, value, firstCell.Position);
                        if (_foundationsController.TryAndAttach(info))
                        {
                            SFXManager.Instance.PlayOneShot(Consts.GetFoundationSFX(seedInGroup));
                        }
                    }
                });
            
                _sequence.Append(groupSequence);
            }

            _sequence.OnComplete(() => IsAnimationActive = false);
            _sequence.Play();
        }

        public void Kill(bool complete)
        {
            _sequence.Kill(complete);
        }

    }
}